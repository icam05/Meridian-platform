export interface Patient {
  patientId: number;
  externalMemberId: string;
  firstName: string;
  lastName: string;
  dateOfBirth?: string;
  sex?: string;
  createdDtm: string;
  updatedDtm: string;
}
