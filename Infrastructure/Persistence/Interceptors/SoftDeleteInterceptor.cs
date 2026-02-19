using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using VsaTemplate.Common.Abstractions;

namespace VsaTemplate.Infrastructure.Persistence.Interceptors;

public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        var context = eventData.Context;
        if (context is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var entries = context
            .ChangeTracker.Entries<ISoftDelete>()
            .Where(e => e.State == EntityState.Deleted);

        foreach (var entry in entries)
        {
            entry.State = EntityState.Modified;

            entry.Entity.IsDeleted = true;
            entry.Entity.DeletedAt = DateTimeOffset.UtcNow;
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
