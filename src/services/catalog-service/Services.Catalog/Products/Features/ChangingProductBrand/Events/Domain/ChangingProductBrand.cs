using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Core.CQRS.Events.Internal;
using ECommerce.Services.Catalogs.Brands;
using Services.Catalog.Brands;
using Services.Catalog.Shared.Contracts;
using Services.Catalog.Shared.Extensions;

namespace Services.Catalog.Products.Features.ChangingProductBrand.Events.Domain;

internal record ChangingProductBrand(BrandId BrandId) : DomainEvent;

internal class ChangingProductBrandValidationHandler :
	IDomainEventHandler<ChangingProductBrand> {
	private readonly ICatalogDbContext _catalogDbContext;

	public ChangingProductBrandValidationHandler(ICatalogDbContext catalogDbContext) {
		_catalogDbContext = catalogDbContext;
	}

	public async Task Handle(ChangingProductBrand notification, CancellationToken cancellationToken) {
		// Handling some validations
		Guard.Against.Null(notification, nameof(notification));
		Guard.Against.NegativeOrZero(notification.BrandId, nameof(notification.BrandId));
		Guard.Against.ExistsBrand(
			await _catalogDbContext.BrandExistsAsync(notification.BrandId, cancellationToken: cancellationToken),
			notification.BrandId);
	}
}
