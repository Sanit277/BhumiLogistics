import { useState, type FormEvent } from "react";
import { api, setToken, currentUserRole, ApiError } from "../api/client";

interface LoginPageProps {
  onLoggedIn: () => void;
}

export default function LoginPage({ onLoggedIn }: LoginPageProps) {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [submitting, setSubmitting] = useState(false);

  async function handleSubmit(event: FormEvent) {
    event.preventDefault();
    setError(null);
    setSubmitting(true);

    try {
      const { token } = await api.login(email, password);
      setToken(token);

      if (currentUserRole() !== "Administrator") {
        setError("This account does not have administrator access.");
        setToken("");
        return;
      }

      onLoggedIn();
    } catch (err) {
      setError(
        err instanceof ApiError ? err.message : "Could not reach the server.",
      );
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <div className="login-screen">
      <div className="login-screen__brand">
        <div className="login-screen__wordmark">
          Bhumi<span>Logistics</span>
        </div>
        <p className="login-screen__tagline">
          A working record of every highway plot listed, every offer made, and
          every lease closed.
        </p>
        <div className="login-screen__meta">
          Control room — internal use only
        </div>
      </div>

      <div className="login-screen__form-wrap">
        <form className="login-form" onSubmit={handleSubmit}>
          <h1>Sign in</h1>
          <p className="subtitle">Administrator access required.</p>

          {error && <div className="form-error">{error}</div>}

          <div className="field">
            <label htmlFor="email">Email</label>
            <input
              id="email"
              type="email"
              autoComplete="username"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </div>

          <div className="field">
            <label htmlFor="password">Password</label>
            <input
              id="password"
              type="password"
              autoComplete="current-password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>

          <button type="submit" className="btn-primary" disabled={submitting}>
            {submitting ? "Signing in…" : "Sign in"}
          </button>

          <p className="login-form__hint">
            Sign in with an account whose role is <strong>Administrator</strong>
            . Other roles will be rejected here even with a valid password.
          </p>
        </form>
      </div>
    </div>
  );
}
