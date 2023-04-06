using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Core.CQRS.Events.Internal;
using ECommerce.Services.Catalogs.Categories;
using Services.Catalog.Categories;
using Services.Catalog.Shared.Contracts;
using Services.Catalog.Shared.Extensions;

namespace Services.Catalog.Products.Features.ChangingProductCategory.Events;

public record ChangingProductCategory(CategoryId CategoryId) : DomainEvent;

internal class ChangingProductCategoryValidationHandler :
	IDomainEventHandler<ChangingProductCategory> {
	private readonly ICatalogDbContext _catalogDbContext;

	public ChangingProductCategoryValidationHandler(ICatalogDbContext catalogDbContext) {
		_catalogDbContext = catalogDbContext;
	}

	public async Task Handle(ChangingProductCategory notification, CancellationToken cancellationToken) {
		Guard.Against.Null(notification, nameof(notification));
		Guard.Against.NegativeOrZero(notification.CategoryId, nameof(notification.CategoryId));
		Guard.Against.ExistsCategory(
			await _catalogDbContext.CategoryExistsAsync(notification.CategoryId, cancellationToken: cancellationToken),
			notification.CategoryId);
	}
}
