import { useEffect, useState, useCallback } from "react";
import { api, clearToken, ApiError } from "../api/client";
import type { AdminUser, AdminLandPlot, AdminLeaseOffer } from "../api/client";
import UsersTable from "../components/UsersTable";
import LandPlotsTable from "../components/LandPlotsTable";
import LeaseOffersTable from "../components/LeaseOffersTable";

type Section = "overview" | "users" | "plots" | "offers";

interface DashboardPageProps {
  onSignedOut: () => void;
}

export default function DashboardPage({ onSignedOut }: DashboardPageProps) {
  const [section, setSection] = useState<Section>("overview");
  const [users, setUsers] = useState<AdminUser[]>([]);
  const [plots, setPlots] = useState<AdminLandPlot[]>([]);
  const [offers, setOffers] = useState<AdminLeaseOffer[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const loadAll = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const [u, p, o] = await Promise.all([
        api.getUsers(),
        api.getLandPlots(),
        api.getLeaseOffers(),
      ]);
      setUsers(u);
      setPlots(p);
      setOffers(o);
    } catch (err) {
      if (err instanceof ApiError && err.status === 401) {
        onSignedOut();
        return;
      }
      setError(
        err instanceof ApiError ? err.message : "Could not reach the server.",
      );
    } finally {
      setLoading(false);
    }
  }, [onSignedOut]);

  useEffect(() => {
    async function loadDashboard() {
      await loadAll();
    }

    void loadDashboard();
  }, [loadAll]);

  async function handleDeletePlot(id: string) {
    await api.deleteLandPlot(id);
    setPlots((prev) => prev.filter((p) => p.id !== id));
  }

  function handleSignOut() {
    clearToken();
    onSignedOut();
  }

  const leasedCount = plots.filter((p) => p.isLeased).length;
  const pendingOffers = offers.filter((o) => o.status === 0).length;

  const sectionMeta: Record<Section, { title: string; subtitle: string }> = {
    overview: {
      title: "Overview",
      subtitle: "A snapshot of listings, tenants, and lease activity.",
    },
    users: {
      title: "Accounts",
      subtitle:
        "Every landowner and corporate tenant registered on the platform.",
    },
    plots: {
      title: "Land plots",
      subtitle: "Every highway-connected parcel listed for lease.",
    },
    offers: {
      title: "Lease offers",
      subtitle: "Every offer submitted by a corporate tenant.",
    },
  };

  return (
    <div className="shell">
      <aside className="rail">
        <div className="rail__wordmark">
          Bhumi<span>Logistics</span>
        </div>
        <nav className="rail__nav">
          <button
            className={`rail__link ${section === "overview" ? "active" : ""}`}
            onClick={() => setSection("overview")}
          >
            Overview
          </button>
          <button
            className={`rail__link ${section === "users" ? "active" : ""}`}
            onClick={() => setSection("users")}
          >
            Accounts ({users.length})
          </button>
          <button
            className={`rail__link ${section === "plots" ? "active" : ""}`}
            onClick={() => setSection("plots")}
          >
            Land plots ({plots.length})
          </button>
          <button
            className={`rail__link ${section === "offers" ? "active" : ""}`}
            onClick={() => setSection("offers")}
          >
            Lease offers ({offers.length})
          </button>
        </nav>
        <div className="rail__footer">
          <button className="rail__signout" onClick={handleSignOut}>
            Sign out
          </button>
        </div>
      </aside>

      <main className="main">
        <div className="main__header">
          <h1>{sectionMeta[section].title}</h1>
          <p>{sectionMeta[section].subtitle}</p>
        </div>

        <div className="stat-strip">
          <div className="stat">
            <div className="stat__value">{plots.length}</div>
            <div className="stat__label">Plots listed</div>
          </div>
          <div className="stat">
            <div className="stat__value">{leasedCount}</div>
            <div className="stat__label">Currently leased</div>
          </div>
          <div className="stat">
            <div className="stat__value">{pendingOffers}</div>
            <div className="stat__label">Offers pending review</div>
          </div>
          <div className="stat">
            <div className="stat__value">{users.length}</div>
            <div className="stat__label">Registered accounts</div>
          </div>
        </div>

        {error && <div className="form-error">{error}</div>}

        {loading ? (
          <div className="table-wrap">
            <table>
              <tbody>
                <tr className="loading-row">
                  <td>Loading records…</td>
                </tr>
              </tbody>
            </table>
          </div>
        ) : (
          <>
            {section === "overview" && (
              <LandPlotsTable
                plots={plots.slice(0, 5)}
                onDelete={handleDeletePlot}
              />
            )}
            {section === "users" && <UsersTable users={users} />}
            {section === "plots" && (
              <LandPlotsTable plots={plots} onDelete={handleDeletePlot} />
            )}
            {section === "offers" && <LeaseOffersTable offers={offers} />}
          </>
        )}
      </main>
    </div>
  );
}
