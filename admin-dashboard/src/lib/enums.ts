// Ordinal positions must stay in sync with the backend enums:
// BhumiLogistics.Domain.Enums.{UserRole, HighwayType, OfferStatus}

export const USER_ROLE_LABELS: Record<number, string> = {
  0: "Landowner",
  1: "Corporate tenant",
  2: "Administrator",
};

export const HIGHWAY_TYPE_LABELS: Record<number, string> = {
  0: "No frontage",
  1: "State highway",
  2: "National highway",
  3: "Expressway",
  4: "Corner, dual frontage",
};

export const OFFER_STATUS_LABELS: Record<number, string> = {
  0: "Pending",
  1: "Under review",
  2: "Accepted",
  3: "Rejected",
  4: "Withdrawn",
  5: "Expired",
};

export function label(map: Record<number, string>, value: number): string {
  return map[value] ?? "Unknown";
}
