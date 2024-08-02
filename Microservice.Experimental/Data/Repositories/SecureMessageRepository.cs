using Dapper;
using Microservice.Experimental.Contracts;
using Microservice.Experimental.Data.Models;
using Microservice.Experimental.Data.Repositories.Abstract;
using Microservice.Experimental.Data.Repositories.Queries;
using Microservice.Experimental.Validation;

namespace Microservice.Experimental.Data.Repositories;


public class SecureMessageRepository(IDbConnectionFactory connectionFactory) :
    BaseRepository(connectionFactory), ISecureMessageRepository
{
    /// <inheritdoc />
    async Task<Either<InternalError, SecureMessage>> ISecureMessageRepository.GetSecureMessageAsync(int id)
    {
        const string query = SecureMessageQueries.GetById;
        var parameters = new DynamicParameters();
        parameters.Add("id", id);
        await using var connection = ConnectionFactory.CreateConnection();
        var result=  await connection.QuerySingleOrDefaultAsync<SecureMessage>(query, parameters);
        return result is null
            ? new Either<InternalError, SecureMessage>(new InternalError
                { Type = ErrorType.NotFound, Message = $"Secure message Id:{id} not found." })
            : new Either<InternalError, SecureMessage>(result);
    }

    /// <inheritdoc />
    async Task<IReadOnlyCollection<SecureMessage>> ISecureMessageRepository.ListSecureMessagesAsync()
    {
        const string query = SecureMessageQueries.GetAll;
        await using var connection = ConnectionFactory.CreateConnection();
        var items = await connection.QueryAsync<SecureMessage>(query);
        return items.ToList().AsReadOnly();
    }

    /// <inheritdoc />
    async Task<Either<InternalError, SecureMessage>> ISecureMessageRepository.InsertSecureMessageAsync(SecureMessage domain)
    {
        const string query= SecureMessageQueries.Insert;
        var parameters = new DynamicParameters();
        parameters.Add("subject", domain.Subject);
        parameters.Add("body",domain.Body);
        parameters.Add("bodyPreview",domain.BodyPreview);
        parameters.Add("status", domain.Status);
        await using var connection = ConnectionFactory.CreateConnection();
        var result = await connection.QuerySingleOrDefaultAsync<SecureMessage>(query, parameters);
        return result is null
            ? new Either<InternalError, SecureMessage>(new InternalError
                { Type = ErrorType.Unknown, Message = "Insert of Secure message failed." })
            : new Either<InternalError, SecureMessage>(result);
    }
}