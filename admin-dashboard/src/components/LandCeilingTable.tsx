import type { LandCeilingFlag } from "../api/client";

export default function LandCeilingTable({
  flags,
}: {
  flags: LandCeilingFlag[];
}) {
  if (flags.length === 0) {
    return (
      <div className="empty-state">
        No owners currently exceed the review threshold.
      </div>
    );
  }

  return (
    <div className="table-wrap">
      <table>
        <thead>
          <tr>
            <th>Owner</th>
            <th>Email</th>
            <th>Declared holding (Kattha)</th>
            <th>Verified listed area (Kattha)</th>
            <th>Review threshold</th>
          </tr>
        </thead>
        <tbody>
          {flags.map((f) => (
            <tr key={f.ownerId}>
              <td>{f.ownerFullName}</td>
              <td>{f.ownerEmail}</td>
              <td className="mono">
                {f.declaredTotalLandHoldingInKattha?.toFixed(2) ?? (
                  <span className="empty-cell">Not declared</span>
                )}
              </td>
              <td className="mono">
                {f.actualVerifiedPlotAreaInKattha.toFixed(2)}
              </td>
              <td className="mono">{f.reviewThresholdInKattha.toFixed(2)}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
