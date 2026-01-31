import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import { getPatient } from "../services/patients";
import type { Patient } from "../types/patient";

export default function PatientDetail() {
  const { id } = useParams();
  const patientId = Number(id);

  const [patient, setPatient] = useState<Patient | null>(null);
  const [error, setError] = useState("");

  useEffect(() => {
    if (!Number.isFinite(patientId)) {
      setError("Invalid patient id");
      return;
    }

    getPatient(patientId)
      .then(setPatient)
      .catch((e) => setError(e?.message ?? "Failed to load patient"));
  }, [patientId]);

  return (
    <div style={{ padding: 24, fontFamily: "Arial" }}>
      <p>
        <Link to="/patients">← Back to Patients</Link>
      </p>

      <h2>Patient Detail</h2>

      {error && <pre>{error}</pre>}

      {!error && !patient && <p>Loading…</p>}

      {patient && (
        <div>
          <p>
            <b>ID:</b> {patient.patientId}
          </p>
          <p>
            <b>Name:</b> {patient.lastName}, {patient.firstName}
          </p>
          <p>
            <b>External Member:</b> {patient.externalMemberId}
          </p>
          <p>
            <b>DOB:</b> {patient.dateOfBirth ?? "-"}
          </p>
          <p>
            <b>Sex:</b> {patient.sex ?? "-"}
          </p>
          <p>
            <b>Created:</b> {patient.createdDtm}
          </p>
          <p>
            <b>Updated:</b> {patient.updatedDtm}
          </p>
        </div>
      )}
    </div>
  );
}
