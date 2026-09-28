import { useEffect, useState } from "react";
import { api } from "../api/client";
import type { PlanQuoteDto } from "../types";

export function PlansPage() {
  const [rows, setRows] = useState<PlanQuoteDto[]>([]);

  useEffect(() => {
    api.plans().then(setRows).catch(() => setRows([]));
  }, []);

  return (
    <section>
      {rows.map((row) => (
        <article key={row.code} className="card">
          <h3>
            {row.name} ({row.code})
          </h3>
          <p>
            {row.tier} · base ${row.monthlyBase.toFixed(2)} · Phoenix quote $
            {row.quotedPhoenixTotal.toFixed(2)}
          </p>
        </article>
      ))}
    </section>
  );
}
