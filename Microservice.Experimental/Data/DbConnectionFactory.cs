using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace Microservice.Experimental.Data;

public sealed class DbConnectionFactory(IConfiguration configuration) : IDbConnectionFactory
{
    private const string MasterWrite = "masterWrite";

    /// <inheritdoc />
    public DbConnection CreateConnection()
    {
        var connectionString = configuration.GetConnectionString(MasterWrite);
        SqlConnectionStringBuilder builder = new(connectionString);

        return new SqlConnection(builder.ToString());
    }
}