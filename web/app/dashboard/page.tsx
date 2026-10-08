import { getSession } from "@/lib/session";

export default async function DashboardPage() {
  const session = await getSession();

  return (
    <main className="max-w-5xl mx-auto px-6 py-16">
      <h1 className="font-display text-3xl font-semibold mb-2">
        Welcome, {session?.fullName}
      </h1>
      <p className="text-ink-dim">
        Signed in as <span className="font-medium">{session?.email}</span>,
        role: <span className="font-medium">{session?.role}</span>
      </p>
    </main>
  );
}
