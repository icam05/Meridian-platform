namespace Meridian.Api.Contracts.PlanEnrollments;

public sealed record PlanEnrollmentDetailDto(
    long PlanEnrollmentId,
    long PatientId,
    int PlanId,
    DateTime EffectiveStartDtm,
    DateTime? EffectiveEndDtm,
    DateTime CreatedDtm,
    DateTime UpdatedDtm
);
