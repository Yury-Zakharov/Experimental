namespace Microservice.Experimental.Validation;

public record InternalError
{
    public ErrorType Type { get; set; }
    public required string Message { get; set; }

    public IResult ToExternal() =>
        Type switch
        {
            ErrorType.Unknown => TypedResults.StatusCode(500),
            ErrorType.NotFound => TypedResults.NotFound(Message),
            ErrorType.Validation => TypedResults.BadRequest(Message),
            _ => throw new ArgumentOutOfRangeException()
        };
}