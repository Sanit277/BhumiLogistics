import type { AdminLeaseOffer } from "../api/client";
import {
  OFFER_STATUS_LABELS,
  MALPOT_REGISTRATION_STATUS_LABELS,
  label,
} from "../lib/enums";

export default function LeaseOffersTable({
  offers,
}: {
  offers: AdminLeaseOffer[];
}) {
  if (offers.length === 0) {
    return (
      <div className="empty-state">
        No lease offers have been submitted yet.
      </div>
    );
  }

  return (
    <div className="table-wrap">
      <table>
        <thead>
          <tr>
            <th>Plot</th>
            <th>Tenant</th>
            <th>Offered amount</th>
            <th>Term</th>
            <th>Status</th>
            <th>Malpot registration</th>
            <th>Est. TDS (10%)</th>
          </tr>
        </thead>
        <tbody>
          {offers.map((o) => (
            <tr key={o.id}>
              <td className="mono">{o.plotPlusCode}</td>
              <td>{o.tenantName}</td>
              <td className="mono">
                {o.offeredAmount.toLocaleString(undefined, {
                  style: "currency",
                  currency: "NPR",
                })}
              </td>
              <td className="mono">{o.durationInYears} yr</td>
              <td>
                <span className={`pill pill--status-${o.status}`}>
                  {label(OFFER_STATUS_LABELS, o.status)}
                </span>
              </td>
              <td>
                <span className="pill pill--role">
                  {label(
                    MALPOT_REGISTRATION_STATUS_LABELS,
                    o.malpotRegistrationStatus,
                  )}
                </span>
                {o.registeredDeedReferenceNumber && (
                  <div className="cell-subtext">
                    {o.registeredDeedReferenceNumber}
                  </div>
                )}
              </td>
              <td className="mono">
                {o.estimatedTdsWithholdingAmount > 0 ? (
                  o.estimatedTdsWithholdingAmount.toLocaleString(undefined, {
                    style: "currency",
                    currency: "NPR",
                  })
                ) : (
                  <span className="empty-cell">—</span>
                )}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
