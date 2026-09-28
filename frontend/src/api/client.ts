import type { LocationDto, PlanQuoteDto, SessionDto, TrainerDto } from "../types";

async function getJson<T>(path: string): Promise<T> {
  const response = await fetch(path);
  if (!response.ok) {
    throw new Error(`Request failed: ${response.status}`);
  }
  return (await response.json()) as T;
}

export const api = {
  locations: () => getJson<LocationDto[]>("/api/locations"),
  plans: () => getJson<PlanQuoteDto[]>("/api/plans"),
  trainers: () => getJson<TrainerDto[]>("/api/trainers"),
  sessions: () => getJson<SessionDto[]>("/api/sessions"),
  join: (body: {
    firstName: string;
    lastName: string;
    email: string;
    dateOfBirth: string;
    homeLocationId: string;
  }) =>
    fetch("/api/members", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(body),
    }),
  book: (body: { memberId: string; sessionId: string; planCode: string }) =>
    fetch("/api/bookings", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(body),
    }),
};
