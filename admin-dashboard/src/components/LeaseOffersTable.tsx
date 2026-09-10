import { AdminLeaseOffer } from "../api/client";
import { OFFER_STATUS_LABELS, label } from "../lib/enums";

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
            <th>Proposed start</th>
            <th>Status</th>
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
              <td className="mono">{o.proposedStartDate}</td>
              <td>
                <span className={`pill pill--status-${o.status}`}>
                  {label(OFFER_STATUS_LABELS, o.status)}
                </span>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
