/** Mirrors backend ArizonaPricingService for client-side quotes. */
export const STATE_TPT = 0.056;

export function quoteMonthlyTotal(
  basePrice: number,
  cityRate: number,
  outdoorPlan: boolean,
  asOf: Date,
): number {
  if (basePrice < 0) {
    throw new Error("basePrice must be >= 0");
  }
  let taxable = basePrice;
  const month = asOf.getUTCMonth() + 1;
  if (outdoorPlan && month >= 6 && month <= 8) {
    taxable = Math.round(basePrice * 0.92 * 100) / 100;
  }
  const combined = STATE_TPT + cityRate;
  return Math.round(taxable * (1 + combined) * 100) / 100;
}

export function isPeakHour(hours: number, minutes: number): boolean {
  const mins = hours * 60 + minutes;
  return (mins >= 5 * 60 + 30 && mins < 8 * 60) || (mins >= 17 * 60 && mins < 19 * 60 + 30);
}

export function mustCancelOutdoor(outdoor: boolean, forecastHighF: number, intensity: number): boolean {
  if (!outdoor) {
    return false;
  }
  if (forecastHighF >= 110) {
    return true;
  }
  return forecastHighF >= 105 && intensity >= 4;
}
