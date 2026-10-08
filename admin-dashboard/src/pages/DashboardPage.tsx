import { useEffect, useState, useCallback } from "react";
import { api, clearToken, ApiError } from "../api/client";
import type {
  AdminUser,
  AdminLandPlot,
  AdminLeaseOffer,
  AdminGrievance,
  LandCeilingFlag,
  HighwaySetbackStandard,
  PlatformDisclosure,
  UpdatePlatformSettingsRequest,
} from "../api/client";
import UsersTable from "../components/UsersTable";
import LandPlotsTable from "../components/LandPlotsTable";
import LeaseOffersTable from "../components/LeaseOffersTable";
import GrievancesTable from "../components/GrievancesTable";
import LandCeilingTable from "../components/LandCeilingTable";
import SettingsPanel from "../components/SettingsPanel";

type Section =
  | "overview"
  | "users"
  | "plots"
  | "offers"
  | "grievances"
  | "ceiling"
  | "settings";

interface DashboardPageProps {
  onSignedOut: () => void;
}

export default function DashboardPage({ onSignedOut }: DashboardPageProps) {
  const [section, setSection] = useState<Section>("overview");
  const [users, setUsers] = useState<AdminUser[]>([]);
  const [plots, setPlots] = useState<AdminLandPlot[]>([]);
  const [offers, setOffers] = useState<AdminLeaseOffer[]>([]);
  const [grievances, setGrievances] = useState<AdminGrievance[]>([]);
  const [ceilingFlags, setCeilingFlags] = useState<LandCeilingFlag[]>([]);
  const [setbackStandards, setSetbackStandards] = useState<
    HighwaySetbackStandard[]
  >([]);
  const [disclosure, setDisclosure] = useState<PlatformDisclosure | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const loadAll = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const [u, p, o, g, c, s, d] = await Promise.all([
        api.getUsers(),
        api.getLandPlots(),
        api.getLeaseOffers(),
        api.getGrievances(),
        api.getLandCeilingReview(),
        api.getSetbackStandards(),
        api.getPlatformDisclosure(),
      ]);
      setUsers(u);
      setPlots(p);
      setOffers(o);
      setGrievances(g);
      setCeilingFlags(c);
      setSetbackStandards(s);
      setDisclosure(d);
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
    loadAll();
  }, [loadAll]);

  async function handleDeletePlot(id: string) {
    await api.deleteLandPlot(id);
    setPlots((prev) => prev.filter((p) => p.id !== id));
  }

  async function handleVerifyPlot(id: string) {
    await api.verifyOwnership(id, null);
    setPlots((prev) =>
      prev.map((p) => (p.id === id ? { ...p, ownershipStatus: 1 } : p)),
    );
  }

  async function handleRejectPlot(id: string, reason: string) {
    await api.rejectOwnership(id, reason);
    setPlots((prev) =>
      prev.map((p) => (p.id === id ? { ...p, ownershipStatus: 2 } : p)),
    );
  }

  async function handleResolveGrievance(id: string, notes: string) {
    await api.resolveGrievance(id, notes);
    setGrievances((prev) =>
      prev.map((g) =>
        g.id === id ? { ...g, status: 2, resolutionNotes: notes } : g,
      ),
    );
  }

  async function handleSaveDisclosure(settings: UpdatePlatformSettingsRequest) {
    await api.updatePlatformSettings(settings);
    const refreshed = await api.getPlatformDisclosure();
    setDisclosure(refreshed);
  }

  async function handleSaveSetback(
    highwayFrontageType: number,
    distance: number,
    notes: string | null,
  ) {
    await api.updateSetbackStandard(highwayFrontageType, distance, notes);
    setSetbackStandards((prev) =>
      prev.map((s) =>
        s.highwayFrontageType === highwayFrontageType
          ? { ...s, setbackDistanceInMeters: distance, notes }
          : s,
      ),
    );
  }

  function handleSignOut() {
    clearToken();
    onSignedOut();
  }

  const leasedCount = plots.filter((p) => p.isLeased).length;
  const pendingVerification = plots.filter(
    (p) => p.ownershipStatus === 0,
  ).length;
  const pendingOffers = offers.filter((o) => o.status === 0).length;
  const openGrievances = grievances.filter(
    (g) => g.status === 0 || g.status === 1,
  ).length;
  const overdueGrievances = grievances.filter((g) => g.isOverdue).length;

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
      subtitle:
        "Every listing, its ownership verification, and land use status.",
    },
    offers: {
      title: "Lease offers",
      subtitle: "Every offer, its registration status, and estimated TDS.",
    },
    grievances: {
      title: "Grievances",
      subtitle:
        "Complaints filed by platform users, with response-time tracking.",
    },
    ceiling: {
      title: "Land ceiling review",
      subtitle:
        "Owners whose declared or verified holdings exceed the review threshold.",
    },
    settings: {
      title: "Settings",
      subtitle: "Public disclosure details and highway setback standards.",
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
          <button
            className={`rail__link ${section === "grievances" ? "active" : ""}`}
            onClick={() => setSection("grievances")}
          >
            Grievances ({openGrievances})
          </button>
          <button
            className={`rail__link ${section === "ceiling" ? "active" : ""}`}
            onClick={() => setSection("ceiling")}
          >
            Land ceiling ({ceilingFlags.length})
          </button>
          <button
            className={`rail__link ${section === "settings" ? "active" : ""}`}
            onClick={() => setSection("settings")}
          >
            Settings
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
            <div className="stat__value">{pendingVerification}</div>
            <div className="stat__label">Awaiting ownership review</div>
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
            <div className="stat__value">{overdueGrievances}</div>
            <div className="stat__label">Overdue grievances</div>
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
                onVerify={handleVerifyPlot}
                onReject={handleRejectPlot}
              />
            )}
            {section === "users" && <UsersTable users={users} />}
            {section === "plots" && (
              <LandPlotsTable
                plots={plots}
                onDelete={handleDeletePlot}
                onVerify={handleVerifyPlot}
                onReject={handleRejectPlot}
              />
            )}
            {section === "offers" && <LeaseOffersTable offers={offers} />}
            {section === "grievances" && (
              <GrievancesTable
                grievances={grievances}
                onResolve={handleResolveGrievance}
              />
            )}
            {section === "ceiling" && <LandCeilingTable flags={ceilingFlags} />}
            {section === "settings" && disclosure && (
              <SettingsPanel
                disclosure={disclosure}
                setbackStandards={setbackStandards}
                onSaveDisclosure={handleSaveDisclosure}
                onSaveSetback={handleSaveSetback}
              />
            )}
          </>
        )}
      </main>
    </div>
  );
}
