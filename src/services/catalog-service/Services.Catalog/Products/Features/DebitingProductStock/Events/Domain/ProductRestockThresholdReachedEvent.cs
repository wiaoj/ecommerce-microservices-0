using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Core.CQRS.Events.Internal;
using Services.Catalog.Products.ValueObjects;
using Services.Catalog.Shared.Contracts;

namespace Services.Catalog.Products.Features.DebitingProductStock.Events.Domain;

public record ProductRestockThresholdReachedEvent(ProductId ProductId, Stock Stock, int Quantity) : DomainEvent;

internal class ProductRestockThresholdReachedEventHandler : IDomainEventHandler<ProductRestockThresholdReachedEvent> {
	private readonly ICatalogDbContext _catalogDbContext;

	public ProductRestockThresholdReachedEventHandler(ICatalogDbContext catalogDbContext) {
		_catalogDbContext = catalogDbContext;
	}

	public Task Handle(ProductRestockThresholdReachedEvent notification, CancellationToken cancellationToken) {
		Guard.Against.Null(notification, nameof(notification));

		// For example send an email to get more products
		return Task.CompletedTask;
	}
}
