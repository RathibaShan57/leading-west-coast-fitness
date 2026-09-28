import { Link, Navigate, Route, Routes } from "react-router-dom";
import { BookingsPage } from "./pages/BookingsPage";
import { HomePage } from "./pages/HomePage";
import { JoinPage } from "./pages/JoinPage";
import { LocationsPage } from "./pages/LocationsPage";
import { PlansPage } from "./pages/PlansPage";
import { TrainersPage } from "./pages/TrainersPage";

export default function App() {
  return (
    <div className="shell">
      <header>
        <p>Arizona · Phoenix metro · Tucson · Flagstaff</p>
        <h1>Leading West Coast Fitness</h1>
        <nav>
          <Link to="/">Home</Link>
          <Link to="/locations">Clubs</Link>
          <Link to="/plans">Memberships</Link>
          <Link to="/trainers">Coaches</Link>
          <Link to="/book">Book a class</Link>
          <Link to="/join">Join</Link>
        </nav>
      </header>
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/locations" element={<LocationsPage />} />
        <Route path="/plans" element={<PlansPage />} />
        <Route path="/trainers" element={<TrainersPage />} />
        <Route path="/book" element={<BookingsPage />} />
        <Route path="/join" element={<JoinPage />} />
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </div>
  );
}
