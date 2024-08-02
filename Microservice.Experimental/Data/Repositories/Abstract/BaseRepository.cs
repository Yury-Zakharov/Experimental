namespace Microservice.Experimental.Data.Repositories.Abstract;

public abstract class BaseRepository
{
    protected IDbConnectionFactory ConnectionFactory { get; }

    protected BaseRepository(IDbConnectionFactory connectionFactory) =>
        ConnectionFactory = connectionFactory;
}