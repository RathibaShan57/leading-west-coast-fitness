import { FormEvent, useEffect, useState } from "react";
import { api } from "../api/client";
import type { SessionDto } from "../types";

export function BookingsPage() {
  const [sessions, setSessions] = useState<SessionDto[]>([]);
  const [memberId, setMemberId] = useState("");
  const [sessionId, setSessionId] = useState("");
  const [planCode, setPlanCode] = useState("CORE");
  const [message, setMessage] = useState<string | null>(null);

  useEffect(() => {
    api.sessions().then((rows) => {
      setSessions(rows);
      if (rows[0]) {
        setSessionId(rows[0].id);
      }
    }).catch(() => setSessions([]));
  }, []);

  async function onSubmit(event: FormEvent) {
    event.preventDefault();
    const result = await api.book({ memberId, sessionId, planCode });
    setMessage(result.ok ? "Booked." : "Booking was not accepted.");
  }

  return (
    <section className="card">
      <form onSubmit={onSubmit}>
        <p>
          <label>
            Member id
            <input value={memberId} onChange={(e) => setMemberId(e.target.value)} required />
          </label>
        </p>
        <p>
          <label>
            Session
            <select value={sessionId} onChange={(e) => setSessionId(e.target.value)}>
              {sessions.map((s) => (
                <option key={s.id} value={s.id}>
                  {new Date(s.startsAt).toLocaleString()} · {s.remaining} open
                </option>
              ))}
            </select>
          </label>
        </p>
        <p>
          <label>
            Plan code
            <input value={planCode} onChange={(e) => setPlanCode(e.target.value)} />
          </label>
        </p>
        <button type="submit">Reserve</button>
      </form>
      {message ? <p>{message}</p> : null}
    </section>
  );
}
