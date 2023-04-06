using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Core.CQRS.Events.Internal;
using ECommerce.Services.Catalogs.Suppliers;
using Services.Catalog.Shared.Contracts;
using Services.Catalog.Shared.Extensions;
using Services.Catalog.Suppliers;

namespace Services.Catalog.Products.Features.ChangingProductSupplier.Events;

public record ChangingProductSupplier(SupplierId SupplierId) : DomainEvent;

internal class ChangingSupplierValidationHandler :
	IDomainEventHandler<ChangingProductSupplier> {
	private readonly ICatalogDbContext _catalogDbContext;

	public ChangingSupplierValidationHandler(ICatalogDbContext catalogDbContext) {
		_catalogDbContext = catalogDbContext;
	}

	public async Task Handle(ChangingProductSupplier notification, CancellationToken cancellationToken) {
		Guard.Against.Null(notification, nameof(notification));
		Guard.Against.NegativeOrZero(notification.SupplierId, nameof(notification.SupplierId));
		Guard.Against.ExistsSupplier(
			await _catalogDbContext.SupplierExistsAsync(notification.SupplierId, cancellationToken: cancellationToken),
			notification.SupplierId);
	}
}
