import type { AdminUser } from "../api/client";
import { USER_ROLE_LABELS, label } from "../lib/enums";

export default function UsersTable({ users }: { users: AdminUser[] }) {
  if (users.length === 0) {
    return <div className="empty-state">No accounts have registered yet.</div>;
  }

  return (
    <div className="table-wrap">
      <table>
        <thead>
          <tr>
            <th>Name</th>
            <th>Email</th>
            <th>Phone</th>
            <th>Role</th>
            <th>Joined</th>
          </tr>
        </thead>
        <tbody>
          {users.map((u) => (
            <tr key={u.id}>
              <td>{u.fullName}</td>
              <td>{u.email}</td>
              <td className="mono">{u.phoneNumber}</td>
              <td>
                <span className="pill pill--role">
                  {label(USER_ROLE_LABELS, u.role)}
                </span>
              </td>
              <td className="mono">
                {new Date(u.createdAtUtc).toLocaleDateString()}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
