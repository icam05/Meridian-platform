using Dapper;
using Microsoft.AspNetCore.Mvc;
using Meridian.Api.Contracts.Patients;
using Meridian.Api.Contracts.PlanEnrollments;
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

    // GET /api/patients/{id}/plan-enrollments
    [HttpGet("{id:long}/plan-enrollments")]
    public async Task<ActionResult<IEnumerable<PlanEnrollmentListItemDto>>> GetPlanEnrollments(long id)
    {
        const string sql = """
            SELECT
                pe.PlanEnrollmentId,
                pe.PatientId,
                (p.LastName + ', ' + p.FirstName) AS PatientName,
                pe.PlanId,
                hp.PlanName,
                pe.EffectiveStartDtm,
                pe.EffectiveEndDtm
            FROM dbo.PlanEnrollment pe
            INNER JOIN dbo.Patient p ON p.PatientId = pe.PatientId
            INNER JOIN dbo.HealthPlan hp ON hp.PlanId = pe.PlanId
            WHERE pe.PatientId = @PatientId
            ORDER BY pe.EffectiveStartDtm DESC, pe.PlanEnrollmentId DESC;
        """;

        using var conn = _factory.CreateConnection();
        var rows = await conn.QueryAsync<PlanEnrollmentListItemDto>(sql, new { PatientId = id });

        return Ok(rows);
    }
    // GET /api/patients/{id}/current-plan-enrollment
    [HttpGet("{id:long}/current-plan-enrollment")]
    public async Task<ActionResult<PlanEnrollmentListItemDto?>> GetCurrentPlanEnrollment(long id)
    {
        const string sql = """
            SELECT TOP (1)
                pe.PlanEnrollmentId,
                pe.PatientId,
                (p.LastName + ', ' + p.FirstName) AS PatientName,
                pe.PlanId,
                hp.PlanName,
                pe.EffectiveStartDtm,
                pe.EffectiveEndDtm
            FROM dbo.PlanEnrollment pe
            INNER JOIN dbo.Patient p ON p.PatientId = pe.PatientId
            INNER JOIN dbo.HealthPlan hp ON hp.PlanId = pe.PlanId
            WHERE pe.PatientId = @PatientId
              AND (
                    pe.EffectiveEndDtm IS NULL
                    OR pe.EffectiveEndDtm >= SYSDATETIME()
                  )
            ORDER BY pe.EffectiveStartDtm DESC, pe.PlanEnrollmentId DESC;
        """;

        using var conn = _factory.CreateConnection();
        var row = await conn.QuerySingleOrDefaultAsync<PlanEnrollmentListItemDto>(
            sql,
            new { PatientId = id }
        );

        return Ok(row); // 200 with null is fine
    }

}
