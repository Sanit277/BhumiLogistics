import { NextRequest, NextResponse } from "next/server";
import { SESSION_COOKIE } from "@/lib/session";

const API_BASE_URL =
  process.env.BHUMI_API_BASE_URL ?? "http://localhost:5139/api";

export async function POST(request: NextRequest) {
  const body = await request.json();

  const apiResponse = await fetch(`${API_BASE_URL}/auth/login`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(body),
  });

  if (!apiResponse.ok) {
    const errorBody = await apiResponse
      .json()
      .catch(() => ({ title: "Login failed" }));
    return NextResponse.json(
      { error: errorBody.title ?? "Login failed" },
      { status: apiResponse.status },
    );
  }

  const { token } = await apiResponse.json();

  const response = NextResponse.json({ success: true });
  response.cookies.set(SESSION_COOKIE, token, {
    httpOnly: true,
    secure: process.env.NODE_ENV === "production",
    sameSite: "lax",
    path: "/",
    maxAge: 60 * 60, // 1 hour, matching the backend's Jwt:ExpiryMinutes
  });

  return response;
}
