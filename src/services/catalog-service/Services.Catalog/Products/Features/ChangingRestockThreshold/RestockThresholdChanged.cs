using BuildingBlocks.Core.CQRS.Events.Internal;

namespace Services.Catalog.Products.Features.ChangingRestockThreshold;

public record RestockThresholdChanged(long ProductId, int RestockThreshold) : DomainEvent;
