import type { AdminLandPlot } from "../api/client";
import { HIGHWAY_TYPE_LABELS, label } from "../lib/enums";

interface LandPlotsTableProps {
  plots: AdminLandPlot[];
  onDelete: (id: string) => void;
}

export default function LandPlotsTable({
  plots,
  onDelete,
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
            <th>Area (Kattha)</th>
            <th>Highway frontage</th>
            <th>Status</th>
            <th>Offers</th>
            <th>Listed</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {plots.map((p) => (
            <tr key={p.id}>
              <td className="mono">{p.plusCode}</td>
              <td className="mono">{p.totalAreaInKattha.toFixed(2)}</td>
              <td>{label(HIGHWAY_TYPE_LABELS, p.highwayFrontageType)}</td>
              <td>
                <span
                  className={`pill ${p.isLeased ? "pill--leased" : "pill--available"}`}
                >
                  {p.isLeased ? "Leased" : "Available"}
                </span>
              </td>
              <td className="mono">{p.offerCount}</td>
              <td className="mono">
                {new Date(p.createdAtUtc).toLocaleDateString()}
              </td>
              <td>
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
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
