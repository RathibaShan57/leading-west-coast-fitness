import { useEffect, useState } from "react";
import { api } from "../api/client";
import type { LocationDto } from "../types";

export function LocationsPage() {
  const [rows, setRows] = useState<LocationDto[]>([]);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    api.locations().then(setRows).catch((err: Error) => setError(err.message));
  }, []);

  if (error) {
    return <p>{error}. Start the API on port 5088.</p>;
  }

  return (
    <section>
      {rows.map((row) => (
        <article key={row.id} className="card">
          <h3>{row.name}</h3>
          <p>
            {row.city}, AZ {row.postalCode} · capacity {row.studioCapacity}
            {row.outdoorPatio ? " · patio" : ""}
          </p>
        </article>
      ))}
    </section>
  );
}
