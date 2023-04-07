using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Abstractions.Domain;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BuildingBlocks.Core.Persistence.EfCore.Interceptors;

// https://khalidabuhakmeh.com/entity-framework-core-5-interceptors
// https://learn.microsoft.com/en-us/ef/core/logging-events-diagnostics/interceptors#savechanges-interception
public class ConcurrencyInterceptor : SaveChangesInterceptor {
	public override ValueTask<InterceptionResult<Int32>> SavingChangesAsync(
		DbContextEventData eventData,
		InterceptionResult<Int32> result,
		CancellationToken cancellationToken) {
		if(eventData.Context is null) {
			return base.SavingChangesAsync(eventData, result, cancellationToken);
		}

		foreach(EntityEntry<IHaveDomainEvents> entry in eventData.Context.ChangeTracker.Entries<IHaveDomainEvents>()) {
			// Ref: http://www.kamilgrzybek.com/design/handling-concurrency-aggregate-pattern-and-ef-core/

			IReadOnlyList<IDomainEvent> events = entry.Entity.GetUncommittedDomainEvents();
			if(events.Any()) {
				if(entry.Entity is IHaveAggregateVersion aggregateVersion) {
					entry.CurrentValues[nameof(IHaveAggregateVersion.OriginalVersion)] = aggregateVersion.OriginalVersion + 1;
				}
			}
		}

		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}
}