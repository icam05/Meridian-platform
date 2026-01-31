using Dapper;
using Microsoft.AspNetCore.Mvc;
using Meridian.Api.Contracts.Patients;
using Meridian.Api.Infrastructure.Data;

namespace Meridian.Api.Controllers;

[ApiController]
[Route("api/patients")]
public sealed class PatientsController : ControllerBase
{
    private readonly ISqlConnectionFactory _factory;

    public PatientsController(ISqlConnectionFactory factory)
    {
        _factory = factory;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientDto>>> List(
        [FromQuery] int skip = 0,
        [FromQuery] int take = 50)
    {
        const string sql = """
            SELECT
                PatientId,
                ExternalMemberId,
                FirstName,
                LastName,
                DateOfBirth,
                Sex,
                CreatedDtm,
                UpdatedDtm
            FROM dbo.Patient
            ORDER BY PatientId
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;
        """;

        using var conn = _factory.CreateConnection();
        var patients = await conn.QueryAsync<PatientDto>(sql, new { Skip = skip, Take = take });

        return Ok(patients);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<PatientDto>> Get(long id)
    {
        const string sql = """
            SELECT
                PatientId,
                ExternalMemberId,
                FirstName,
                LastName,
                DateOfBirth,
                Sex,
                CreatedDtm,
                UpdatedDtm
            FROM dbo.Patient
            WHERE PatientId = @Id;
        """;

        using var conn = _factory.CreateConnection();
        var patient = await conn.QuerySingleOrDefaultAsync<PatientDto>(sql, new { Id = id });

        if (patient is null)
            return NotFound();

        return Ok(patient);
    }
}
