using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Abstractions.Messaging.Bus;
using Services.Catalog.Products.Features.CreatingProduct.v1.Events.Domain;

namespace Services.Catalog.Products.Features.CreatingProduct.v1.Events.Notification;
public record ProductCreatedNotification(ProductCreated DomainEvent)
	: BuildingBlocks.Core.CQRS.Events.Internal.DomainNotificationEventWrapper<ProductCreated>(DomainEvent) {
	public Guid Id => this.DomainEvent.Product.Id;
	public String Name => this.DomainEvent.Product.Name;
	public Guid CategoryId => this.DomainEvent.Product.CategoryId.Value;
	public String? CategoryName => this.DomainEvent.Product.Category?.Name;
	public Int32 Stock => this.DomainEvent.Product.Stock.Available;
}

internal class ProductCreatedNotificationHandler : IDomainNotificationEventHandler<ProductCreatedNotification> {
	private readonly IBus bus;

	public ProductCreatedNotificationHandler(IBus bus) {
		this.bus = bus;
	}

	public async Task Handle(ProductCreatedNotification notification, CancellationToken cancellationToken) {
		// We could publish integration event to bus here
		// await _bus.PublishAsync(
		//     new ECommerce.Services.Shared.Catalogs.Products.Events.Integration.ProductCreatedV1(
		//         notification.InternalCommandId,
		//         notification.Name,
		//         notification.Stock,
		//         notification.CategoryName ?? "",
		//         notification.Stock),
		//     null,
		//     cancellationToken);

		return;
	}
}