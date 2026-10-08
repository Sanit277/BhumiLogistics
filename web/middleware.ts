import { NextRequest, NextResponse } from "next/server";

// Duplicated here rather than imported from lib/session.ts, because
// middleware runs in a restricted "Edge" runtime that can't use the same
// cookies() API as Server Components — it reads request.cookies directly instead.
const SESSION_COOKIE_NAME = "bhumi_session";
const PROTECTED_PREFIXES = ["/dashboard"];

export function middleware(request: NextRequest) {
  const isProtected = PROTECTED_PREFIXES.some((prefix) =>
    request.nextUrl.pathname.startsWith(prefix),
  );

  if (!isProtected) {
    return NextResponse.next();
  }

  const token = request.cookies.get(SESSION_COOKIE_NAME)?.value;

  if (!token) {
    const loginUrl = new URL("/login", request.url);
    loginUrl.searchParams.set("redirectTo", request.nextUrl.pathname);
    return NextResponse.redirect(loginUrl);
  }

  return NextResponse.next();
}

export const config = {
  matcher: ["/dashboard/:path*"],
};
