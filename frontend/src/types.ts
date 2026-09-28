export type LocationDto = {
  id: string;
  name: string;
  city: string;
  postalCode: string;
  studioCapacity: number;
  outdoorPatio: boolean;
};

export type PlanQuoteDto = {
  code: string;
  name: string;
  tier: string;
  monthlyBase: number;
  quotedPhoenixTotal: number;
};

export type TrainerDto = {
  id: string;
  fullName: string;
  specialty: string;
  yearsExperience: number;
};

export type SessionDto = {
  id: string;
  fitnessClassId: string;
  startsAt: string;
  remaining: number;
  cancelled: boolean;
};
