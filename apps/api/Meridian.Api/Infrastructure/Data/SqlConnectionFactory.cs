using System.Data;
using Microsoft.Data.SqlClient;

namespace Meridian.Api.Infrastructure.Data;

public sealed class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString =
            configuration.GetConnectionString("SqlServer")
            ?? throw new InvalidOperationException("Missing connection string 'SqlServer'.");
    }

    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}
