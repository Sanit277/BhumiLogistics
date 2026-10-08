// Ordinal positions must stay in sync with the backend enums in BhumiLogistics.Domain.Enums

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

export const OWNERSHIP_STATUS_LABELS: Record<number, string> = {
  0: "Pending review",
  1: "Verified",
  2: "Rejected",
};

export const TENURE_TYPE_LABELS: Record<number, string> = {
  0: "Raikar",
  1: "Guthi (Raitan Numbari)",
  2: "Guthi (other)",
  3: "Government",
  4: "Unknown",
};

export const LAND_USE_CLASSIFICATION_LABELS: Record<number, string> = {
  0: "Agricultural",
  1: "Residential",
  2: "Commercial",
  3: "Industrial",
  4: "Other",
};

export const MALPOT_REGISTRATION_STATUS_LABELS: Record<number, string> = {
  0: "Not registered",
  1: "Pending registration",
  2: "Registered",
};

export const GRIEVANCE_STATUS_LABELS: Record<number, string> = {
  0: "Open",
  1: "In progress",
  2: "Resolved",
  3: "Closed",
};

export const OWNER_TYPE_LABELS: Record<number, string> = {
  0: "Individual",
  1: "Company",
};

export function label(map: Record<number, string>, value: number): string {
  return map[value] ?? "Unknown";
}
