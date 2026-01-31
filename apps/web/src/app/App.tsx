import { BrowserRouter, Routes, Route, Link } from "react-router-dom";
import PatientsList from "../pages/PatientsList";
import PatientDetail from "../pages/PatientDetail";

export default function App() {
  return (
    <BrowserRouter>
      <nav>
        <Link to="/patients">Patients</Link>
      </nav>

      <Routes>
        <Route path="/patients" element={<PatientsList />} />
        <Route path="/patients/:id" element={<PatientDetail />} />
      </Routes>
    </BrowserRouter>
  );
}
