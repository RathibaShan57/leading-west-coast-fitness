import { isPeakHour, mustCancelOutdoor, quoteMonthlyTotal } from "./arizonaTax";

describe("arizona tax and heat rules", () => {
  it("quotes Phoenix TPT on a core plan", () => {
    expect(quoteMonthlyTotal(100, 0.022, false, new Date("2026-01-15T00:00:00Z"))).toBe(107.8);
  });

  it("discounts outdoor plans in July", () => {
    const winter = quoteMonthlyTotal(179, 0.022, true, new Date("2026-01-15T00:00:00Z"));
    const summer = quoteMonthlyTotal(179, 0.022, true, new Date("2026-07-04T00:00:00Z"));
    expect(summer).toBeLessThan(winter);
  });

  it("detects peak hours", () => {
    expect(isPeakHour(6, 0)).toBe(true);
    expect(isPeakHour(12, 0)).toBe(false);
  });

  it("cancels outdoor HIIT at 105F", () => {
    expect(mustCancelOutdoor(true, 105, 4)).toBe(true);
    expect(mustCancelOutdoor(false, 118, 5)).toBe(false);
  });
});
