import { FormEvent, useEffect, useState } from "react";
import { api } from "../api/client";
import type { LocationDto } from "../types";

export function JoinPage() {
  const [locations, setLocations] = useState<LocationDto[]>([]);
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [email, setEmail] = useState("");
  const [dateOfBirth, setDateOfBirth] = useState("1995-06-01");
  const [homeLocationId, setHomeLocationId] = useState("");
  const [message, setMessage] = useState<string | null>(null);

  useEffect(() => {
    api.locations().then((rows) => {
      setLocations(rows);
      if (rows[0]) {
        setHomeLocationId(rows[0].id);
      }
    }).catch(() => setLocations([]));
  }, []);

  async function onSubmit(event: FormEvent) {
    event.preventDefault();
    const result = await api.join({ firstName, lastName, email, dateOfBirth, homeLocationId });
    setMessage(result.ok ? "Welcome to Leading West Coast Fitness." : "Could not register.");
  }

  return (
    <section className="card">
      <form onSubmit={onSubmit}>
        <p>
          <input placeholder="First name" value={firstName} onChange={(e) => setFirstName(e.target.value)} required />
        </p>
        <p>
          <input placeholder="Last name" value={lastName} onChange={(e) => setLastName(e.target.value)} required />
        </p>
        <p>
          <input type="email" placeholder="Email" value={email} onChange={(e) => setEmail(e.target.value)} required />
        </p>
        <p>
          <input type="date" value={dateOfBirth} onChange={(e) => setDateOfBirth(e.target.value)} required />
        </p>
        <p>
          <select value={homeLocationId} onChange={(e) => setHomeLocationId(e.target.value)}>
            {locations.map((l) => (
              <option key={l.id} value={l.id}>
                {l.name} — {l.city}
              </option>
            ))}
          </select>
        </p>
        <button type="submit">Join the club</button>
      </form>
      {message ? <p>{message}</p> : null}
    </section>
  );
}
