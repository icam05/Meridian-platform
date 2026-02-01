using Dapper;
using Microsoft.AspNetCore.Mvc;
using Meridian.Api.Infrastructure.Data;
using Meridian.Api.Contracts.PlanEnrollments;

namespace Meridian.Api.Controllers;

[ApiController]
[Route("api/plan-enrollments")]
public sealed class PlanEnrollmentsController : ControllerBase
{
    private readonly ISqlConnectionFactory _factory;

    public PlanEnrollmentsController(ISqlConnectionFactory factory)
        => _factory = factory;

    // GET /api/plan-enrollments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlanEnrollmentListItemDto>>> GetAll()
    {
        const string sql = """
            SELECT TOP (200)
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
            ORDER BY pe.EffectiveStartDtm DESC, pe.PlanEnrollmentId DESC;
            """;

        using var conn = _factory.CreateConnection();
        var rows = await conn.QueryAsync<PlanEnrollmentListItemDto>(sql);
        return Ok(rows);
    }

    // GET /api/plan-enrollments/{id}
    [HttpGet("{id:long}")]
    public async Task<ActionResult<PlanEnrollmentDetailDto>> GetById(long id)
    {
        const string sql = """
            SELECT
                PlanEnrollmentId,
                PatientId,
                PlanId,
                EffectiveStartDtm,
                EffectiveEndDtm,
                CreatedDtm,
                UpdatedDtm
            FROM dbo.PlanEnrollment
            WHERE PlanEnrollmentId = @Id;
            """;

        using var conn = _factory.CreateConnection();
        var row = await conn.QuerySingleOrDefaultAsync<PlanEnrollmentDetailDto>(sql, new { Id = id });

        return row is null ? NotFound() : Ok(row);
    }
}
