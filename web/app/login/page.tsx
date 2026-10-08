"use client";

import { useState, FormEvent } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";

export default function LoginPage() {
  const router = useRouter();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [submitting, setSubmitting] = useState(false);

  async function handleSubmit(event: FormEvent) {
    event.preventDefault();
    setError(null);
    setSubmitting(true);

    try {
      const response = await fetch("/api/auth/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
      });

      if (!response.ok) {
        const data = await response
          .json()
          .catch(() => ({ error: "Login failed" }));
        setError(data.error ?? "Login failed");
        return;
      }

      router.push("/dashboard");
      router.refresh();
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <main className="max-w-md mx-auto px-6 py-20">
      <h1 className="font-display text-3xl font-semibold mb-2">Sign in</h1>
      <p className="text-ink-dim mb-8">Welcome back to BhumiLogistics.</p>

      {error && (
        <div className="bg-brick/10 border border-brick text-brick px-4 py-3 rounded mb-6 text-sm">
          {error}
        </div>
      )}

      <form onSubmit={handleSubmit} className="space-y-5">
        <div>
          <label className="block text-sm text-ink-dim mb-1.5">Email</label>
          <input
            type="email"
            required
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            className="w-full border border-hairline rounded px-3 py-2.5 bg-surface"
          />
        </div>
        <div>
          <label className="block text-sm text-ink-dim mb-1.5">Password</label>
          <input
            type="password"
            required
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            className="w-full border border-hairline rounded px-3 py-2.5 bg-surface"
          />
        </div>
        <button
          type="submit"
          disabled={submitting}
          className="w-full bg-forest text-white py-3 rounded font-medium hover:bg-forest/90 disabled:opacity-60"
        >
          {submitting ? "Signing in…" : "Sign in"}
        </button>
      </form>

      <p className="text-sm text-ink-dim mt-6">
        Don&apos;t have an account?{" "}
        <Link href="/register" className="text-accent-dark font-medium">
          Register
        </Link>
      </p>
    </main>
  );
}
