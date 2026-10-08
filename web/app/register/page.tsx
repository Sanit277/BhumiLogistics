"use client";

import { useState, FormEvent } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";

type Role = "landowner" | "tenant";

export default function RegisterPage() {
  const router = useRouter();
  const [role, setRole] = useState<Role | null>(null);

  const [fullName, setFullName] = useState("");
  const [email, setEmail] = useState("");
  const [phoneNumber, setPhoneNumber] = useState("");
  const [password, setPassword] = useState("");

  const [ownerType, setOwnerType] = useState<"individual" | "company">(
    "individual",
  );
  const [declaredLandHolding, setDeclaredLandHolding] = useState("");

  const [companyType, setCompanyType] = useState<"domestic" | "foreign">(
    "domestic",
  );
  const [fittaReference, setFittaReference] = useState("");
  const [doiReference, setDoiReference] = useState("");
  const [extensionApproved, setExtensionApproved] = useState(false);

  const [error, setError] = useState<string | null>(null);
  const [submitting, setSubmitting] = useState(false);

  async function handleSubmit(event: FormEvent) {
    event.preventDefault();
    setError(null);

    if (!role) {
      setError(
        "Please choose whether you're listing land or looking to lease it.",
      );
      return;
    }

    setSubmitting(true);

    try {
      const payload = {
        fullName,
        email,
        phoneNumber,
        password,
        role: role === "landowner" ? 0 : 1,
        ownerType:
          role === "landowner" ? (ownerType === "individual" ? 0 : 1) : null,
        declaredTotalLandHoldingInKattha:
          role === "landowner" && declaredLandHolding
            ? Number(declaredLandHolding)
            : null,
        companyType:
          role === "tenant" ? (companyType === "domestic" ? 0 : 1) : null,
        fittaApprovalReferenceNumber:
          role === "tenant" && companyType === "foreign"
            ? fittaReference
            : null,
        departmentOfIndustryApprovalReferenceNumber:
          role === "tenant" && companyType === "foreign" ? doiReference : null,
        foreignInvestmentExtensionApproved:
          role === "tenant" && companyType === "foreign"
            ? extensionApproved
            : false,
      };

      const registerResponse = await fetch("/api/auth/register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload),
      });

      if (!registerResponse.ok) {
        const data = await registerResponse
          .json()
          .catch(() => ({ error: "Registration failed" }));
        setError(data.error ?? "Registration failed");
        return;
      }

      // Registration succeeded but doesn't return a session — log in immediately
      // afterward so the person lands in their dashboard without a second form.
      const loginResponse = await fetch("/api/auth/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
      });

      if (!loginResponse.ok) {
        router.push("/login");
        return;
      }

      router.push("/dashboard");
      router.refresh();
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <main className="max-w-xl mx-auto px-6 py-16">
      <h1 className="font-display text-3xl font-semibold mb-2">
        Create an account
      </h1>
      <p className="text-ink-dim mb-8">
        Join BhumiLogistics as a landowner or a corporate tenant.
      </p>

      {error && (
        <div className="bg-brick/10 border border-brick text-brick px-4 py-3 rounded mb-6 text-sm">
          {error}
        </div>
      )}

      {/* Step 1: role selection */}
      <div className="grid grid-cols-2 gap-4 mb-8">
        <button
          type="button"
          onClick={() => setRole("landowner")}
          className={`text-left border rounded-lg p-5 transition ${
            role === "landowner"
              ? "border-forest bg-forest-dim"
              : "border-hairline bg-surface hover:border-forest/50"
          }`}
        >
          <div className="font-display text-lg font-semibold mb-1">
            I own land
          </div>
          <div className="text-sm text-ink-dim">
            List highway-connected land for lease
          </div>
        </button>

        <button
          type="button"
          onClick={() => setRole("tenant")}
          className={`text-left border rounded-lg p-5 transition ${
            role === "tenant"
              ? "border-forest bg-forest-dim"
              : "border-hairline bg-surface hover:border-forest/50"
          }`}
        >
          <div className="font-display text-lg font-semibold mb-1">
            I'm looking to lease land
          </div>
          <div className="text-sm text-ink-dim">
            Browse and submit offers on listed plots
          </div>
        </button>
      </div>

      {role && (
        <form onSubmit={handleSubmit} className="space-y-5">
          <div>
            <label className="block text-sm text-ink-dim mb-1.5">
              Full name
            </label>
            <input
              required
              value={fullName}
              onChange={(e) => setFullName(e.target.value)}
              className="w-full border border-hairline rounded px-3 py-2.5 bg-surface"
            />
          </div>

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
            <label className="block text-sm text-ink-dim mb-1.5">
              Phone number
            </label>
            <input
              required
              value={phoneNumber}
              onChange={(e) => setPhoneNumber(e.target.value)}
              className="w-full border border-hairline rounded px-3 py-2.5 bg-surface"
            />
          </div>

          <div>
            <label className="block text-sm text-ink-dim mb-1.5">
              Password
            </label>
            <input
              type="password"
              required
              minLength={8}
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              className="w-full border border-hairline rounded px-3 py-2.5 bg-surface"
            />
            <p className="text-xs text-ink-dim mt-1">At least 8 characters.</p>
          </div>

          {role === "landowner" && (
            <div className="border-t border-hairline pt-5 space-y-5">
              <div>
                <label className="block text-sm text-ink-dim mb-1.5">
                  Are you registering as
                </label>
                <div className="flex gap-4">
                  <label className="flex items-center gap-2 text-sm">
                    <input
                      type="radio"
                      checked={ownerType === "individual"}
                      onChange={() => setOwnerType("individual")}
                    />
                    An individual
                  </label>
                  <label className="flex items-center gap-2 text-sm">
                    <input
                      type="radio"
                      checked={ownerType === "company"}
                      onChange={() => setOwnerType("company")}
                    />
                    A company
                  </label>
                </div>
              </div>

              <div>
                <label className="block text-sm text-ink-dim mb-1.5">
                  Total land you own, in Kattha (optional)
                </label>
                <input
                  type="number"
                  min={0}
                  value={declaredLandHolding}
                  onChange={(e) => setDeclaredLandHolding(e.target.value)}
                  className="w-full border border-hairline rounded px-3 py-2.5 bg-surface"
                />
                <p className="text-xs text-ink-dim mt-1">
                  Used for land-ceiling compliance review. You can leave this
                  blank for now.
                </p>
              </div>
            </div>
          )}

          {role === "tenant" && (
            <div className="border-t border-hairline pt-5 space-y-5">
              <div>
                <label className="block text-sm text-ink-dim mb-1.5">
                  Your company is
                </label>
                <div className="flex gap-4">
                  <label className="flex items-center gap-2 text-sm">
                    <input
                      type="radio"
                      checked={companyType === "domestic"}
                      onChange={() => setCompanyType("domestic")}
                    />
                    Domestic (Nepali-owned)
                  </label>
                  <label className="flex items-center gap-2 text-sm">
                    <input
                      type="radio"
                      checked={companyType === "foreign"}
                      onChange={() => setCompanyType("foreign")}
                    />
                    Foreign-invested
                  </label>
                </div>
              </div>

              {companyType === "foreign" && (
                <div className="space-y-5 bg-forest-dim/40 border border-hairline rounded-lg p-4">
                  <p className="text-sm text-ink-dim">
                    Foreign-invested tenants need FITTA and Department of
                    Industry approval on file before leasing land, per the
                    Foreign Investment and Technology Transfer Act 2019.
                  </p>
                  <div>
                    <label className="block text-sm text-ink-dim mb-1.5">
                      FITTA approval reference
                    </label>
                    <input
                      required
                      value={fittaReference}
                      onChange={(e) => setFittaReference(e.target.value)}
                      className="w-full border border-hairline rounded px-3 py-2.5 bg-surface"
                    />
                  </div>
                  <div>
                    <label className="block text-sm text-ink-dim mb-1.5">
                      Department of Industry / Investment Board approval
                      reference
                    </label>
                    <input
                      required
                      value={doiReference}
                      onChange={(e) => setDoiReference(e.target.value)}
                      className="w-full border border-hairline rounded px-3 py-2.5 bg-surface"
                    />
                  </div>
                  <label className="flex items-center gap-2 text-sm">
                    <input
                      type="checkbox"
                      checked={extensionApproved}
                      onChange={(e) => setExtensionApproved(e.target.checked)}
                    />
                    We hold an approved lease-term extension (allows up to 75
                    years instead of 50)
                  </label>
                </div>
              )}
            </div>
          )}

          <button
            type="submit"
            disabled={submitting}
            className="w-full bg-forest text-white py-3 rounded font-medium hover:bg-forest/90 disabled:opacity-60"
          >
            {submitting ? "Creating account…" : "Create account"}
          </button>
        </form>
      )}

      <p className="text-sm text-ink-dim mt-6">
        Already have an account?{" "}
        <Link href="/login" className="text-accent-dark font-medium">
          Sign in
        </Link>
      </p>
    </main>
  );
}
