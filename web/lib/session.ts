import { cookies } from "next/headers";

const SESSION_COOKIE_NAME = "bhumi_session";

export interface SessionPayload {
  userId: string;
  email: string;
  fullName: string;
  role: string;
}

/**
 * Reads and decodes the JWT stored in the httpOnly cookie. This decodes the
 * payload only — it does NOT verify the signature. That's intentional: the
 * .NET API re-verifies the token's signature on every request it receives,
 * so this is purely for deciding what the UI should render, not a security
 * boundary by itself.
 */
export async function getSession(): Promise<SessionPayload | null> {
  const cookieStore = await cookies();
  const token = cookieStore.get(SESSION_COOKIE_NAME)?.value;
  if (!token) return null;

  try {
    const payloadBase64 = token.split(".")[1];
    const payload = JSON.parse(
      Buffer.from(payloadBase64, "base64").toString("utf-8"),
    );

    return {
      userId:
        payload[
          "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
        ],
      email:
        payload[
          "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
        ],
      fullName:
        payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"],
      role: payload[
        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
      ],
    };
  } catch {
    return null;
  }
}

export async function getRawToken(): Promise<string | null> {
  const cookieStore = await cookies();
  return cookieStore.get(SESSION_COOKIE_NAME)?.value ?? null;
}

export const SESSION_COOKIE = SESSION_COOKIE_NAME;
