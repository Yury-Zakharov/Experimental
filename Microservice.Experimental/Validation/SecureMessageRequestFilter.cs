using Microservice.Experimental.Contracts;

namespace Microservice.Experimental.Validation;

public sealed class SecureMessageRequestFilter(ILoggerFactory loggerFactory) : IEndpointFilter
{
    private readonly ILogger logger = loggerFactory.CreateLogger<SecureMessageRequestFilter>();
    /// <inheritdoc />
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var request = context.GetArgument<SecureMessageRequest>(0);
        if (string.IsNullOrWhiteSpace(request.Subject))
        {
            logger.LogError("Request subject is empty.");
            return Results.BadRequest("Invalid subject.");
        }

        return await next(context);
    }
}