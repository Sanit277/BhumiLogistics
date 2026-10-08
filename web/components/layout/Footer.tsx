import Link from "next/link";
import { connection } from "next/server";

export default async function Footer() {
  await connection();
  const currentYear = new Date().getFullYear();

  return (
    <footer className="border-t border-hairline mt-24 py-10 px-6 text-sm text-ink-dim">
      <div className="max-w-6xl mx-auto flex flex-col md:flex-row justify-between gap-4">
        <p>&copy; {currentYear} BhumiLogistics. All rights reserved.</p>
        <div className="flex gap-6">
          <Link href="/legal" className="hover:text-accent-dark">
            Legal disclosure
          </Link>
          <Link href="/dashboard/grievances" className="hover:text-accent-dark">
            File a grievance
          </Link>
        </div>
      </div>
    </footer>
  );
}
