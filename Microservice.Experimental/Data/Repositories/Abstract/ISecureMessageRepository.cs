using Microservice.Experimental.Contracts;
using Microservice.Experimental.Data.Models;
using Microservice.Experimental.Validation;

namespace Microservice.Experimental.Data.Repositories.Abstract;

public interface ISecureMessageRepository
{
    Task<Either<InternalError, SecureMessage>> GetSecureMessageAsync(int id);
    Task<IReadOnlyCollection<SecureMessage>> ListSecureMessagesAsync();
    Task<Either<InternalError, SecureMessage>> InsertSecureMessageAsync(SecureMessage domain);
}