import Link from "next/link";
import { getSession } from "@/lib/session";
import LogoutButton from "./LogoutButton";

export default async function Header() {
  const session = await getSession();

  return (
    <header className="border-b border-hairline bg-surface">
      <div className="max-w-6xl mx-auto px-6 py-4 flex items-center justify-between">
        <Link href="/" className="font-display text-xl font-semibold">
          Bhumi<span className="text-accent-dark">Logistics</span>
        </Link>

        <nav className="flex items-center gap-6 text-sm">
          <Link href="/listings" className="hover:text-accent-dark">
            Browse land
          </Link>
          <Link href="/legal" className="hover:text-accent-dark">
            Legal
          </Link>

          {session ? (
            <>
              <Link href="/dashboard" className="hover:text-accent-dark">
                Dashboard
              </Link>
              <span className="text-ink-dim">
                Hi, {session.fullName.split(" ")[0]}
              </span>
              <LogoutButton />
            </>
          ) : (
            <>
              <Link href="/login" className="hover:text-accent-dark">
                Log in
              </Link>
              <Link
                href="/register"
                className="bg-forest text-white px-4 py-2 rounded font-medium hover:bg-forest/90"
              >
                Register
              </Link>
            </>
          )}
        </nav>
      </div>
    </header>
  );
}
