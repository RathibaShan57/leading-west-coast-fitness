import { useEffect, useState } from "react";
import { api } from "../api/client";
import type { TrainerDto } from "../types";

export function TrainersPage() {
  const [rows, setRows] = useState<TrainerDto[]>([]);
  useEffect(() => {
    api.trainers().then(setRows).catch(() => setRows([]));
  }, []);
  return (
    <section>
      {rows.map((row) => (
        <article key={row.id} className="card">
          <h3>{row.fullName}</h3>
          <p>
            {row.specialty} · {row.yearsExperience} years
          </p>
        </article>
      ))}
    </section>
  );
}
