import type { AdminGrievance } from "../api/client";
import { GRIEVANCE_STATUS_LABELS, label } from "../lib/enums";

interface GrievancesTableProps {
  grievances: AdminGrievance[];
  onResolve: (id: string, notes: string) => void;
}

export default function GrievancesTable({
  grievances,
  onResolve,
}: GrievancesTableProps) {
  if (grievances.length === 0) {
    return <div className="empty-state">No grievances have been filed.</div>;
  }

  return (
    <div className="table-wrap">
      <table>
        <thead>
          <tr>
            <th>Subject</th>
            <th>Description</th>
            <th>Submitted</th>
            <th>Expected response by</th>
            <th>Status</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {grievances.map((g) => (
            <tr key={g.id}>
              <td>{g.subject}</td>
              <td className="cell-truncate">{g.description}</td>
              <td className="mono">
                {new Date(g.submittedAtUtc).toLocaleDateString()}
              </td>
              <td className="mono">
                {new Date(g.expectedResponseByUtc).toLocaleDateString()}
                {g.isOverdue && (
                  <span className="pill pill--leased overdue-tag">Overdue</span>
                )}
              </td>
              <td>
                <span
                  className={`pill ${g.status === 2 ? "pill--available" : "pill--role"}`}
                >
                  {label(GRIEVANCE_STATUS_LABELS, g.status)}
                </span>
              </td>
              <td>
                {g.status !== 2 && g.status !== 3 && (
                  <button
                    className="btn-small btn-approve"
                    onClick={() => {
                      const notes = window.prompt("Resolution notes:");
                      if (notes) onResolve(g.id, notes);
                    }}
                  >
                    Resolve
                  </button>
                )}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
