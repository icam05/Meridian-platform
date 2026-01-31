import { api } from "./api";
import type { Patient } from "../types/patient";

export async function listPatients(skip = 0, take = 50) {
  const res = await api.get<Patient[]>("/api/patients", {
    params: { skip, take }
  });
  return res.data;
}

export async function getPatient(id: number) {
  const res = await api.get<Patient>(`/api/patients/${id}`);
  return res.data;
}
