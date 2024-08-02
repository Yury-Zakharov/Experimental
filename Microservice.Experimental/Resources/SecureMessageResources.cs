using Microservice.Experimental.Contracts;
using Microservice.Experimental.Data.Models;
using Microservice.Experimental.Data.Repositories.Abstract;
using Microservice.Experimental.Mappers;
using Microservice.Experimental.Validation;

namespace Microservice.Experimental.Resources;

public static class SecureMessageResources
{
    public static async Task<IResult> GetMessage(int id, ISecureMessageRepository repository)
    {
        Either<InternalError, SecureMessage> either = await repository.GetSecureMessageAsync(id);

        return either.Map(SecureMessageMapper.ToContract)
            .Match(
                e => e.ToExternal(),
                m => TypedResults.Ok(m)
            );
    }

    public static async Task<IResult> CreateMessage(SecureMessageRequest request, ISecureMessageRepository repository)
    {
        Either<InternalError, SecureMessage> either = await repository.InsertSecureMessageAsync(request.MapToDomain());
        return either.Map(SecureMessageMapper.ToContract)
            .Match<IResult>(
                e => e.ToExternal(),
                m => TypedResults.Created($"/messages/{m.Id}", m)
            );
    }

    public static async Task<IResult> ListMessages(ISecureMessageRepository repository) =>
         TypedResults.Ok(await repository.ListSecureMessagesAsync());

}