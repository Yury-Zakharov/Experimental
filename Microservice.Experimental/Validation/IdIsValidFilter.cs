namespace Microservice.Experimental.Validation;

/// <summary>
/// Every effish one loves Check.IsTrue(Id > 0) everywhere in the code, from top to bottom,
/// <para>So I've created an endpoint filter for this check.</para>
/// <para>This is only example, in reality we should not check this condition ever.</para>
/// </summary>
/// <param name="loggerFactory"></param>
public sealed class IdIsValidFilter(ILoggerFactory loggerFactory) : IEndpointFilter
{
    private readonly ILogger logger = loggerFactory.CreateLogger<IdIsValidFilter>();

    /// <inheritdoc />
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var id = context.GetArgument<int>(0);
        if (id <= 0)
        {
            logger.LogError($"Endpoint {context.HttpContext.Request.Method} {context.HttpContext.Request.Path}: Invalid identifier {context.HttpContext.Request.RouteValues.First()}");
            return Results.BadRequest("Invalid identifier.");
        }

        return await next(context);
    }
}