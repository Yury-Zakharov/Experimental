using System.Data.Common;

namespace Microservice.Experimental.Data;

public interface IDbConnectionFactory
{
    DbConnection CreateConnection();
}