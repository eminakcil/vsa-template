using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using VsaTemplate.Common.Abstractions;

namespace VsaTemplate.Infrastructure.Persistence.Interceptors;

public class AuditInterceptor : SaveChangesInterceptor
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

        var entries = context.ChangeTracker.Entries<IAuditable>();

        foreach (var entry in entries)
        {
            var now = DateTimeOffset.UtcNow;

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
