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
  return response.json();
}

export interface LoginResponse {
  token: string;
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
};

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
  highwayFrontageType: number;
  isLeased: boolean;
  ownerId: string;
  offerCount: number;
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
  createdAtUtc: string;
}
