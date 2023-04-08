using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Core.CQRS.Events.Internal;
using Services.Catalog.Products.Exceptions.Application;
using Services.Catalog.Products.Models;
using Services.Catalog.Shared.Data;

namespace Services.Catalog.Products.Features.CreatingProduct.v1.Events.Domain;
public record ProductCreated(Product Product) : DomainEvent;

internal class ProductCreatedHandler : IDomainEventHandler<ProductCreated> {
	private readonly CatalogDbContext dbContext;

	public ProductCreatedHandler(CatalogDbContext dbContext) {
		this.dbContext = dbContext;
	}

	public async Task Handle(ProductCreated notification, CancellationToken cancellationToken) {
		ArgumentNullException.ThrowIfNull(notification, nameof(notification));

		ProductView? existed = await this.dbContext.ProductsView.SingleOrDefaultAsync(
			x => x.ProductId == notification.Product.Id.Value,
			cancellationToken
		);

		if(existed is null) {
			Product product = await this.dbContext.Products
				.Include(x => x.Brand)
				.Include(x => x.Category)
				.Include(x => x.Supplier)
				.SingleOrDefaultAsync(x => x.Id == notification.Product.Id, cancellationToken) 
				?? throw new ProductNotFoundException(notification.Product.Id);

			ProductView productView = new() {
				ProductId = product!.Id,
				ProductName = product.Name,
				CategoryId = product.CategoryId,
				CategoryName = product.Category?.Name ?? String.Empty,
				SupplierId = product.SupplierId,
				SupplierName = product.Supplier?.Name ?? String.Empty,
				BrandId = product.BrandId,
				BrandName = product.Brand?.Name ?? String.Empty,
			};

			await this.dbContext.Set<ProductView>().AddAsync(productView, cancellationToken);
			await this.dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}

// Mapping domain event to integration event in domain event handler is better from mapping in command handler (for preserving our domain rule invariants).
internal class ProductCreatedDomainEventToIntegrationMappingHandler : IDomainEventHandler<ProductCreated> {
	public ProductCreatedDomainEventToIntegrationMappingHandler() { }

	public Task Handle(ProductCreated domainEvent, CancellationToken cancellationToken) {
		// 1. Mapping DomainEvent To IntegrationEvent
		// 2. Save Integration Event to Outbox
		return Task.CompletedTask;
	}
}