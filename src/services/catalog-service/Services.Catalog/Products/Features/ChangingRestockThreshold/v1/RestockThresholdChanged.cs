using BuildingBlocks.Core.CQRS.Events.Internal;

namespace Services.Catalog.Products.Features.ChangingRestockThreshold.v1;
public record RestockThresholdChanged(Guid ProductId, int RestockThreshold) : DomainEvent;