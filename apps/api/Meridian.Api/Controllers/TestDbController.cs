using Dapper;
using Microsoft.AspNetCore.Mvc;
using Meridian.Api.Infrastructure.Data;

namespace Meridian.Api.Controllers;

[ApiController]
[Route("api/test-db")]
public class TestDbController : ControllerBase
{
    private readonly ISqlConnectionFactory _factory;

    public TestDbController(ISqlConnectionFactory factory)
        => _factory = factory;

    [HttpGet]
    public async Task<IActionResult> TestDb()
    {
        using var conn = _factory.CreateConnection();

        var info = await conn.QuerySingleAsync(
      @"SELECT
    DB_NAME() AS DatabaseName,
    (SELECT COUNT(*) FROM sys.tables) AS TableCount,
    (SELECT COUNT(*) FROM sys.views) AS ViewCount,
    (SELECT COUNT(*) FROM sys.procedures) AS ProcCount,
    (SELECT COUNT(*) FROM sys.schemas) AS SchemaCount;"
    );

        return Ok(info);


        /*
                var result = await conn.ExecuteScalarAsync<int>(
                    "SELECT COUNT(*) FROM sys.tables"
                );

                return Ok(new
                {
                    databaseReachable = true,
                    tableCount = result,
                    utc = DateTime.UtcNow
                });  */
    }
}
