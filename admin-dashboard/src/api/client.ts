const API_BASE_URL = "http://localhost:5139/api";

export class ApiError extends Error {
  status: number;

  constructor(status: number, message: string) {
    super(message);
    this.status = status;
  }
}

function getToken(): string | null {
  return localStorage.getItem("bhumi_admin_token");
}

export function setToken(token: string): void {
  localStorage.setItem("bhumi_admin_token", token);
}

export function clearToken(): void {
  localStorage.removeItem("bhumi_admin_token");
}

export function isLoggedIn(): boolean {
  return getToken() !== null;
}

/** Decodes the JWT payload (no verification — the server already verifies it on every request). */
export function currentUserRole(): string | null {
  const token = getToken();
  if (!token) return null;

  try {
    const payload = JSON.parse(atob(token.split(".")[1]));
    return (
      payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] ??
      null
    );
  } catch {
    return null;
  }
}

async function request<T>(path: string, options: RequestInit = {}): Promise<T> {
  const token = getToken();

  const response = await fetch(`${API_BASE_URL}${path}`, {
    ...options,
    headers: {
      "Content-Type": "application/json",
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
      ...options.headers,
    },
  });

  if (!response.ok) {
    const body = await response
      .json()
      .catch(() => ({ title: response.statusText }));
    throw new ApiError(response.status, body.title ?? "Request failed");
  }

  if (response.status === 204) return undefined as T;
  return response.json() as Promise<T>;
}

export interface LoginResponse {
  token: string;
}

// The API serializes C# enums as their underlying numeric value (default System.Text.Json
// behaviour), so these fields arrive as numbers, not strings.
export interface AdminUser {
  id: string;
  fullName: string;
  email: string;
  phoneNumber: string;
  role: number;
  createdAtUtc: string;
}

export interface AdminLandPlot {
  id: string;
  plusCode: string;
  totalAreaInKattha: number;
  buildableAreaInKattha: number;
  highwayFrontageType: number;
  isLeased: boolean;
  ownerId: string;
  offerCount: number;
  ownershipStatus: number;
  registeredOwnerName: string | null;
  tenureType: number;
  mohiTenancyDeclared: boolean;
  landUseClassification: number;
  frontageLengthInMeters: number;
  setbackDistanceInMeters: number;
  createdAtUtc: string;
}

export interface AdminLeaseOffer {
  id: string;
  landPlotId: string;
  plotPlusCode: string;
  tenantName: string;
  offeredAmount: number;
  durationInYears: number;
  status: number;
  proposedStartDate: string;
  malpotRegistrationStatus: number;
  registeredDeedReferenceNumber: string | null;
  registrationDate: string | null;
  ownerType: number | null;
  estimatedTdsWithholdingAmount: number;
  createdAtUtc: string;
}

export interface AdminGrievance {
  id: string;
  submittedByUserId: string;
  subject: string;
  description: string;
  status: number;
  submittedAtUtc: string;
  expectedResponseByUtc: string;
  isOverdue: boolean;
  resolutionNotes: string | null;
}

export interface LandCeilingFlag {
  ownerId: string;
  ownerFullName: string;
  ownerEmail: string;
  declaredTotalLandHoldingInKattha: number | null;
  actualVerifiedPlotAreaInKattha: number;
  reviewThresholdInKattha: number;
}

export interface HighwaySetbackStandard {
  highwayFrontageType: number;
  setbackDistanceInMeters: number;
  notes: string | null;
}

export interface PlatformDisclosure {
  businessName: string;
  panNumber: string;
  vatNumber: string | null;
  registeredAddress: string;
  contactEmail: string;
  contactPhone: string;
  grievanceOfficerName: string;
  grievanceOfficerEmail: string;
  grievanceOfficerPhone: string;
}

export interface UpdatePlatformSettingsRequest {
  businessName: string;
  panNumber: string;
  vatNumber: string | null;
  registeredAddress: string;
  contactEmail: string;
  contactPhone: string;
  grievanceOfficerName: string;
  grievanceOfficerEmail: string;
  grievanceOfficerPhone: string;
  landCeilingReviewThresholdInKattha: number;
}

export const api = {
  login: (email: string, password: string) =>
    request<LoginResponse>("/auth/login", {
      method: "POST",
      body: JSON.stringify({ email, password }),
    }),

  getUsers: () => request<AdminUser[]>("/admin/users"),
  getLandPlots: () => request<AdminLandPlot[]>("/admin/land-plots"),
  getLeaseOffers: () => request<AdminLeaseOffer[]>("/admin/lease-offers"),
  deleteLandPlot: (id: string) =>
    request<void>(`/admin/land-plots/${id}`, { method: "DELETE" }),
  getGrievances: () => request<AdminGrievance[]>("/admin/grievances"),
  getLandCeilingReview: () =>
    request<LandCeilingFlag[]>("/admin/land-ceiling-review"),
  getSetbackStandards: () =>
    request<HighwaySetbackStandard[]>("/admin/highway-setback-standards"),
  getPlatformDisclosure: () =>
    request<PlatformDisclosure>("/platform-disclosure"),
  verifyOwnership: (id: string, notes: string | null) =>
    request<void>(`/admin/land-plots/${id}/verify-ownership`, {
      method: "PUT",
      body: JSON.stringify({ notes }),
    }),
  rejectOwnership: (id: string, reason: string) =>
    request<void>(`/admin/land-plots/${id}/reject-ownership`, {
      method: "PUT",
      body: JSON.stringify({ reason }),
    }),
  resolveGrievance: (id: string, notes: string) =>
    request<void>(`/admin/grievances/${id}/resolve`, {
      method: "PUT",
      body: JSON.stringify({ resolutionNotes: notes }),
    }),
  updatePlatformSettings: (settings: UpdatePlatformSettingsRequest) =>
    request<void>("/admin/platform-settings", {
      method: "PUT",
      body: JSON.stringify(settings),
    }),
  updateSetbackStandard: (
    highwayFrontageType: number,
    setbackDistanceInMeters: number,
    notes: string | null,
  ) =>
    request<void>(`/admin/highway-setback-standards/${highwayFrontageType}`, {
      method: "PUT",
      body: JSON.stringify({ setbackDistanceInMeters, notes }),
    }),
};
