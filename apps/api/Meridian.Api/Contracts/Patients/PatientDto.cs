namespace Meridian.Api.Contracts.Patients;

public sealed record PatientDto(
    long PatientId,
    string ExternalMemberId,
    string FirstName,
    string LastName,
    DateTime? DateOfBirth,
    string? Sex,
    DateTime CreatedDtm,
    DateTime UpdatedDtm
);
