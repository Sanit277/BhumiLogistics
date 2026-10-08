import { useState } from "react";
import type {
  HighwaySetbackStandard,
  PlatformDisclosure,
  UpdatePlatformSettingsRequest,
} from "../api/client";
import { HIGHWAY_TYPE_LABELS, label } from "../lib/enums";

interface SettingsPanelProps {
  disclosure: PlatformDisclosure;
  setbackStandards: HighwaySetbackStandard[];
  onSaveDisclosure: (settings: UpdatePlatformSettingsRequest) => Promise<void>;
  onSaveSetback: (
    highwayFrontageType: number,
    distance: number,
    notes: string | null,
  ) => Promise<void>;
}

export default function SettingsPanel({
  disclosure,
  setbackStandards,
  onSaveDisclosure,
  onSaveSetback,
}: SettingsPanelProps) {
  const [form, setForm] = useState({
    businessName: disclosure.businessName,
    panNumber: disclosure.panNumber,
    vatNumber: disclosure.vatNumber ?? "",
    registeredAddress: disclosure.registeredAddress,
    contactEmail: disclosure.contactEmail,
    contactPhone: disclosure.contactPhone,
    grievanceOfficerName: disclosure.grievanceOfficerName,
    grievanceOfficerEmail: disclosure.grievanceOfficerEmail,
    grievanceOfficerPhone: disclosure.grievanceOfficerPhone,
    landCeilingReviewThresholdInKattha: "100",
  });
  const [saving, setSaving] = useState(false);
  const [savedMessage, setSavedMessage] = useState<string | null>(null);

  function updateField(field: keyof typeof form, value: string) {
    setForm((prev) => ({ ...prev, [field]: value }));
  }

  async function handleSave() {
    setSaving(true);
    setSavedMessage(null);
    try {
      await onSaveDisclosure({
        ...form,
        vatNumber: form.vatNumber || null,
        landCeilingReviewThresholdInKattha: Number(
          form.landCeilingReviewThresholdInKattha,
        ),
      });
      setSavedMessage("Saved.");
    } finally {
      setSaving(false);
    }
  }

  return (
    <div className="settings-layout">
      <section className="settings-section">
        <h2>Platform disclosure</h2>
        <p className="settings-section__note">
          Publicly visible at <code>/api/platform-disclosure</code>, required
          under the Electronic Commerce Act 2081. The land ceiling threshold
          below is internal-only (not public).
        </p>

        <div className="settings-grid">
          <div className="field">
            <label>Business name</label>
            <input
              value={form.businessName}
              onChange={(e) => updateField("businessName", e.target.value)}
            />
          </div>
          <div className="field">
            <label>PAN number</label>
            <input
              value={form.panNumber}
              onChange={(e) => updateField("panNumber", e.target.value)}
            />
          </div>
          <div className="field">
            <label>VAT number (optional)</label>
            <input
              value={form.vatNumber}
              onChange={(e) => updateField("vatNumber", e.target.value)}
            />
          </div>
          <div className="field">
            <label>Registered address</label>
            <input
              value={form.registeredAddress}
              onChange={(e) => updateField("registeredAddress", e.target.value)}
            />
          </div>
          <div className="field">
            <label>Contact email</label>
            <input
              value={form.contactEmail}
              onChange={(e) => updateField("contactEmail", e.target.value)}
            />
          </div>
          <div className="field">
            <label>Contact phone</label>
            <input
              value={form.contactPhone}
              onChange={(e) => updateField("contactPhone", e.target.value)}
            />
          </div>
          <div className="field">
            <label>Grievance officer name</label>
            <input
              value={form.grievanceOfficerName}
              onChange={(e) =>
                updateField("grievanceOfficerName", e.target.value)
              }
            />
          </div>
          <div className="field">
            <label>Grievance officer email</label>
            <input
              value={form.grievanceOfficerEmail}
              onChange={(e) =>
                updateField("grievanceOfficerEmail", e.target.value)
              }
            />
          </div>
          <div className="field">
            <label>Grievance officer phone</label>
            <input
              value={form.grievanceOfficerPhone}
              onChange={(e) =>
                updateField("grievanceOfficerPhone", e.target.value)
              }
            />
          </div>
          <div className="field">
            <label>Land ceiling review threshold (Kattha)</label>
            <input
              type="number"
              value={form.landCeilingReviewThresholdInKattha}
              onChange={(e) =>
                updateField(
                  "landCeilingReviewThresholdInKattha",
                  e.target.value,
                )
              }
            />
          </div>
        </div>

        <button
          className="btn-primary settings-save"
          onClick={handleSave}
          disabled={saving}
        >
          {saving ? "Saving…" : "Save disclosure settings"}
        </button>
        {savedMessage && (
          <span className="settings-saved-tag">{savedMessage}</span>
        )}
      </section>

      <section className="settings-section">
        <h2>Highway setback standards</h2>
        <p className="settings-section__note">
          Used to compute buildable area on new listings. Regulatory figures are
          currently in flux — update these as confirmed.
        </p>

        <div className="table-wrap">
          <table>
            <thead>
              <tr>
                <th>Highway type</th>
                <th>Setback (meters)</th>
                <th>Notes</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              {setbackStandards.map((s) => (
                <SetbackRow
                  key={s.highwayFrontageType}
                  standard={s}
                  onSave={onSaveSetback}
                />
              ))}
            </tbody>
          </table>
        </div>
      </section>
    </div>
  );
}

function SetbackRow({
  standard,
  onSave,
}: {
  standard: HighwaySetbackStandard;
  onSave: (
    highwayFrontageType: number,
    distance: number,
    notes: string | null,
  ) => Promise<void>;
}) {
  const [distance, setDistance] = useState(
    String(standard.setbackDistanceInMeters),
  );
  const [notes, setNotes] = useState(standard.notes ?? "");
  const [saving, setSaving] = useState(false);

  async function handleSave() {
    setSaving(true);
    try {
      await onSave(
        standard.highwayFrontageType,
        Number(distance),
        notes || null,
      );
    } finally {
      setSaving(false);
    }
  }

  return (
    <tr>
      <td>{label(HIGHWAY_TYPE_LABELS, standard.highwayFrontageType)}</td>
      <td>
        <input
          className="inline-input"
          type="number"
          value={distance}
          onChange={(e) => setDistance(e.target.value)}
        />
      </td>
      <td>
        <input
          className="inline-input inline-input--wide"
          value={notes}
          onChange={(e) => setNotes(e.target.value)}
        />
      </td>
      <td>
        <button
          className="btn-small btn-approve"
          onClick={handleSave}
          disabled={saving}
        >
          {saving ? "Saving…" : "Save"}
        </button>
      </td>
    </tr>
  );
}
