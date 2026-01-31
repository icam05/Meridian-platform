import { useEffect, useState } from "react";
import { api } from "../services/api";

type TestDbInfo = {
  databaseName: string;
  tableCount: number;
  viewCount: number;
  procCount: number;
  schemaCount: number;
};

export default function App() {
  const [data, setData] = useState<TestDbInfo | null>(null);
  const [error, setError] = useState("");

  useEffect(() => {
    api
      .get<TestDbInfo>("/api/test-db")
      .then((r) => setData(r.data))
      .catch((e) => setError(e?.message ?? "Request failed"));
  }, []);

  return (
    <div style={{ padding: 24, fontFamily: "Arial" }}>
      <h1>Meridian Platform</h1>

      {error && <pre>{error}</pre>}

      {data ? (
        <pre>{JSON.stringify(data, null, 2)}</pre>
      ) : (
        !error && <p>Loading…</p>
      )}
    </div>
  );
}
