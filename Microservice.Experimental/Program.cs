using DbUp;
using IntelliFlo.Platform.Security;
using Microservice.Experimental.Contracts;
using Microservice.Experimental.Data;
using Microservice.Experimental.Data.Repositories;
using Microservice.Experimental.Data.Repositories.Abstract;
using Microservice.Experimental.Validation;
using System.Reflection;
using Microservice.Experimental.Platform;
using static Microservice.Experimental.Resources.SecureMessageResources;


var builder = MinimalMicroserviceApp.Build<Startup>(args);

builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<ISecureMessageRepository, SecureMessageRepository>();

#region Database init

// Initialise the database.
// Can be moved to separate module.
var connectionString = builder.Configuration.GetConnectionString("masterWrite");
// Database will be created if not exist.
EnsureDatabase.For.SqlDatabase(connectionString);

var upgradeEngine = DeployChanges
    .To
    .SqlDatabase(connectionString)
    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
    .LogToConsole()
    .Build();

var result = upgradeEngine.PerformUpgrade();
if (!result.Successful)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(result.Error);
    Console.ResetColor();
#if DEBUG
    Console.ReadLine();
#endif
    return -1;
}
#endregion

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

Lazy<string> extraDocumentationPath = new(() => Path.GetFullPath(Path.Combine(GetAssemblyDirectory() ?? string.Empty, "Microservice.Experimental.Contracts.xml")));
Lazy<string> mainDocumentationPath = new(() => Path.GetFullPath(Path.Combine(GetAssemblyDirectory() ?? string.Empty, "Microservice.Experimental.xml")));
//builder.Services.AddAuthorization();
//builder.Services.AddAuthorizationBuilder()
//    .AddPolicy(PolicyNames.FirmData, policy => policy.RequireClaim("scope", Scopes.FirmData));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(mainDocumentationPath.Value);
    options.IncludeXmlComments(extraDocumentationPath.Value);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/messages/{id}", GetMessage)
    .RequireAuthorization(PolicyNames.FirmData)
    .AddEndpointFilter<IdIsValidFilter>()
    .WithName("GetSecureMessage")
    .WithTags("secure messages", "alternative")
    .Produces<SecureMessageDocument>()
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status404NotFound)
    .WithOpenApi();

app.MapGet("/messages", ListMessages)
    .Produces<IReadOnlyCollection<SecureMessageDocument>>()
    .WithName("ListSecureMessages")
    .WithTags("secure messages", "alternative")
    .WithOpenApi();

app.MapPost("/messages", CreateMessage)
    .AddEndpointFilter<SecureMessageRequestFilter>()
    .WithName("CreateSecureMessage")
    .WithTags("secure messages", "alternative")
    .Accepts<SecureMessageRequest>("application/json")
    .Produces<SecureMessageDocument>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .WithOpenApi();
app.Run();

return 0;
static string? GetAssemblyDirectory()
{
    // Assembly.CodeBase is deprecated in .Net 5.0
    // Have to be careful with location because
    // on Windows it will have value like C:\work\Microservice.Entitlement\test\Microservice.Entitlement\bin\release\net5.0\Microservice.Entitlement.dll
    // but on linux (i.e. in Harness subsystem workflow) it will be /test/bin/Release/net5.0/Microservice.Entitlement.dll
    // this is an ambiguous for UriBuilder(string) which will throw a UriFormatException because it can't determine the host
    // so we have to help it out by pre-pending "file://"
    var location = typeof(Program).Assembly.Location;
    if (string.IsNullOrEmpty(location))
        return null;

    UriBuilder uri = new($"file://{location}");
    var path = Uri.UnescapeDataString(uri.Path);
    return Path.GetDirectoryName(path);
}