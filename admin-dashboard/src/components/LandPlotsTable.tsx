import type { AdminLandPlot } from "../api/client";
import {
  HIGHWAY_TYPE_LABELS,
  OWNERSHIP_STATUS_LABELS,
  LAND_USE_CLASSIFICATION_LABELS,
  label,
} from "../lib/enums";

interface LandPlotsTableProps {
  plots: AdminLandPlot[];
  onDelete: (id: string) => void;
  onVerify: (id: string) => void;
  onReject: (id: string, reason: string) => void;
}

function ownershipPillClass(status: number): string {
  if (status === 1) return "pill--available"; // Verified
  if (status === 2) return "pill--leased"; // Rejected
  return "pill--role"; // Pending review — neutral
}

export default function LandPlotsTable({
  plots,
  onDelete,
  onVerify,
  onReject,
}: LandPlotsTableProps) {
  if (plots.length === 0) {
    return <div className="empty-state">No plots have been listed yet.</div>;
  }

  return (
    <div className="table-wrap">
      <table>
        <thead>
          <tr>
            <th>Plus Code</th>
            <th>Owner (declared)</th>
            <th>Area (total / buildable)</th>
            <th>Highway</th>
            <th>Land use</th>
            <th>Mohi?</th>
            <th>Ownership status</th>
            <th>Lease status</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {plots.map((p) => (
            <tr key={p.id}>
              <td className="mono">{p.plusCode}</td>
              <td>
                {p.registeredOwnerName || <span className="empty-cell">—</span>}
              </td>
              <td className="mono">
                {p.totalAreaInKattha.toFixed(2)} /{" "}
                {p.buildableAreaInKattha.toFixed(2)}
              </td>
              <td>{label(HIGHWAY_TYPE_LABELS, p.highwayFrontageType)}</td>
              <td>
                {label(LAND_USE_CLASSIFICATION_LABELS, p.landUseClassification)}
              </td>
              <td>{p.mohiTenancyDeclared ? "Yes" : "No"}</td>
              <td>
                <span
                  className={`pill ${ownershipPillClass(p.ownershipStatus)}`}
                >
                  {label(OWNERSHIP_STATUS_LABELS, p.ownershipStatus)}
                </span>
              </td>
              <td>
                <span
                  className={`pill ${p.isLeased ? "pill--leased" : "pill--available"}`}
                >
                  {p.isLeased ? "Leased" : "Available"}
                </span>
              </td>
              <td>
                <div className="row-actions">
                  {p.ownershipStatus === 0 && (
                    <>
                      <button
                        className="btn-small btn-approve"
                        onClick={() => onVerify(p.id)}
                      >
                        Verify
                      </button>
                      <button
                        className="btn-small btn-danger"
                        onClick={() => {
                          const reason = window.prompt(
                            "Reason for rejecting this plot\u2019s ownership:",
                          );
                          if (reason) onReject(p.id, reason);
                        }}
                      >
                        Reject
                      </button>
                    </>
                  )}
                  <button
                    className="btn-danger"
                    onClick={() => {
                      if (
                        window.confirm(
                          `Remove listing ${p.plusCode}? This cannot be undone.`,
                        )
                      ) {
                        onDelete(p.id);
                      }
                    }}
                  >
                    Remove
                  </button>
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
