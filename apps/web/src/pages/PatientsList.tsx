import { useEffect, useState } from "react";
import { listPatients } from "../services/patients";
import type { Patient } from "../types/patient";
import { Link } from "react-router-dom";

export default function PatientsList() {
  const [patients, setPatients] = useState<Patient[]>([]);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    listPatients()
      .then(setPatients)
      .catch(() => setError("Failed to load patients"));
  }, []);

  if (error) return <p>{error}</p>;

  return (
    <>
      <h2>Patients</h2>
      <table>
        <thead>
          <tr>
            <th>PatientId</th>
            <th>Name</th>
            <th>External Member</th>
          </tr>
        </thead>
        <tbody>
          {patients.map((p) => (
            <tr key={p.patientId}>
              <td>{p.patientId}</td>
              <td>
                <Link to={`/patients/${p.patientId}`}>
                  {p.lastName}, {p.firstName}
                </Link>
              </td>
              <td>{p.externalMemberId}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </>
  );
}
