import { NextRequest, NextResponse } from "next/server";

const API_BASE_URL =
  process.env.BHUMI_API_BASE_URL ?? "http://localhost:5139/api";

export async function POST(request: NextRequest) {
  const body = await request.json();

  const apiResponse = await fetch(`${API_BASE_URL}/auth/register`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(body),
  });

  if (!apiResponse.ok) {
    const errorBody = await apiResponse
      .json()
      .catch(() => ({ title: "Registration failed" }));
    return NextResponse.json(
      { error: errorBody.title ?? "Registration failed" },
      { status: apiResponse.status },
    );
  }

  const data = await apiResponse.json();
  return NextResponse.json(data, { status: 201 });
}
