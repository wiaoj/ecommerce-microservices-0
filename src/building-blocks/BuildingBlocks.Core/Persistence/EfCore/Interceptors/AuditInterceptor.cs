using BuildingBlocks.Abstractions.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BuildingBlocks.Core.Persistence.EfCore.Interceptors;

// https://khalidabuhakmeh.com/entity-framework-core-5-interceptors
// https://learn.microsoft.com/en-us/ef/core/logging-events-diagnostics/interceptors#savechanges-interception
// Ref: https://www.meziantou.net/entity-framework-core-generate-tracking-columns.htm
public class AuditInterceptor : SaveChangesInterceptor {
	public override ValueTask<InterceptionResult<Int32>> SavingChangesAsync(
		DbContextEventData eventData,
		InterceptionResult<Int32> result,
		CancellationToken cancellationToken) {

		if(eventData.Context is null) {
			return base.SavingChangesAsync(eventData, result, cancellationToken);
		}

		DateTime utcNow = DateTime.UtcNow;

		// var userId = GetCurrentUser(); // TODO: Get current user
		foreach(EntityEntry<IHaveAudit> entry in eventData.Context.ChangeTracker.Entries<IHaveAudit>()) {
			switch(entry.State) {
				case EntityState.Modified:
					entry.CurrentValues[nameof(IHaveAudit.LastModified)] = utcNow;
					entry.CurrentValues[nameof(IHaveAudit.LastModifiedBy)] = 1;
					break;
				case EntityState.Added:
					entry.CurrentValues[nameof(IHaveAudit.Created)] = utcNow;
					entry.CurrentValues[nameof(IHaveAudit.CreatedBy)] = 1;
					break;
			}
		}

		foreach(EntityEntry<IHaveCreator> entry in eventData.Context.ChangeTracker.Entries<IHaveCreator>()) {
			if(entry.State is EntityState.Added) {
				entry.CurrentValues[nameof(IHaveCreator.Created)] = utcNow;
				entry.CurrentValues[nameof(IHaveCreator.CreatedBy)] = 1;
			}
		}

		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}
}