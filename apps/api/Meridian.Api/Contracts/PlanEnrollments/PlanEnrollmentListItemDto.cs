namespace Meridian.Api.Contracts.PlanEnrollments;

public sealed record PlanEnrollmentListItemDto(
    long PlanEnrollmentId,
    long PatientId,
    string PatientName,
    int PlanId,
    string PlanName,
    DateTime EffectiveStartDtm,
    DateTime? EffectiveEndDtm
);
