using BhumiLogistics.Domain.Common;
using BhumiLogistics.Domain.Enums;
using BhumiLogistics.Domain.Exceptions;

namespace BhumiLogistics.Domain.Entities;

/// <summary>
/// A complaint filed by a platform user. Tracks the ~15-day response window
/// the Electronic Commerce Act 2081 expects from an intermediary platform.
/// SubmittedAtUtc is set explicitly here rather than relying on the base
/// entity's CreatedAtUtc, since that field is not yet reliably populated
/// (a known gap to fix separately with a proper audit-stamping interceptor).
/// </summary>
public class Grievance : BaseAuditableEntity
{
    private const int ExpectedResponseWindowDays = 15;

    public Guid SubmittedByUserId { get; private set; }
    public string Subject { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public GrievanceStatus Status { get; private set; }
    public DateTimeOffset SubmittedAtUtc { get; private set; }
    public Guid? RespondedByAdminUserId { get; private set; }
    public string? ResolutionNotes { get; private set; }
    public DateTimeOffset? ResolvedAtUtc { get; private set; }

    public DateTimeOffset ExpectedResponseByUtc => SubmittedAtUtc.AddDays(ExpectedResponseWindowDays);

    public bool IsOverdue =>
        Status is GrievanceStatus.Open or GrievanceStatus.InProgress
        && DateTimeOffset.UtcNow > ExpectedResponseByUtc;

    private Grievance() { } // EF Core

    private Grievance(Guid submittedByUserId, string subject, string description)
    {
        SubmittedByUserId = submittedByUserId;
        Subject = subject;
        Description = description;
        Status = GrievanceStatus.Open;
        SubmittedAtUtc = DateTimeOffset.UtcNow;
    }

    public static Grievance Submit(Guid submittedByUserId, string subject, string description)
    {
        if (string.IsNullOrWhiteSpace(subject))
            throw new DomainException("A grievance must include a subject.");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("A grievance must include a description of the issue.");

        return new Grievance(submittedByUserId, subject, description);
    }

    public void MarkInProgress()
    {
        if (Status != GrievanceStatus.Open)
            throw new DomainException("Only an open grievance can be marked in progress.");

        Status = GrievanceStatus.InProgress;
    }

    public void Resolve(Guid adminUserId, string resolutionNotes)
    {
        if (Status == GrievanceStatus.Closed)
            throw new DomainException("A closed grievance cannot be resolved.");

        if (string.IsNullOrWhiteSpace(resolutionNotes))
            throw new DomainException("Resolution notes are required when resolving a grievance.");

        Status = GrievanceStatus.Resolved;
        RespondedByAdminUserId = adminUserId;
        ResolutionNotes = resolutionNotes;
        ResolvedAtUtc = DateTimeOffset.UtcNow;
    }
}