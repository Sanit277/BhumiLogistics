using BhumiLogistics.Application.Common.Interfaces;
using BhumiLogistics.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BhumiLogistics.Infrastructure.Persistence.Interceptors;

/// <summary>
/// Automatically stamps CreatedAtUtc/CreatedBy and LastModifiedAtUtc/LastModifiedBy
/// on every BaseAuditableEntity as part of SaveChanges — so no command handler needs
/// to remember to set these fields manually, and they can never be silently missed.
/// </summary>
public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService _currentUserService;

    public AuditableEntitySaveChangesInterceptor(ICurrentUserService currentUserService) =>
        _currentUserService = currentUserService;

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateAuditableEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateAuditableEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAuditableEntities(DbContext? context)
    {
        if (context is null) return;

        var utcNow = DateTimeOffset.UtcNow;
        var currentUserId = _currentUserService.UserId?.ToString();

        foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAtUtc = utcNow;
                entry.Entity.CreatedBy = currentUserId;
            }

            // Also catches cases where only an owned value object (like OwnershipVerification
            // or LandUseDeclaration) changed, without the parent entity's own scalar properties changing.
            if (entry.State is EntityState.Added or EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                entry.Entity.LastModifiedAtUtc = utcNow;
                entry.Entity.LastModifiedBy = currentUserId;
            }
        }
    }
}

file static class EntityEntryExtensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry is not null
            && r.TargetEntry.Metadata.IsOwned()
            && r.TargetEntry.State is EntityState.Added or EntityState.Modified);
}