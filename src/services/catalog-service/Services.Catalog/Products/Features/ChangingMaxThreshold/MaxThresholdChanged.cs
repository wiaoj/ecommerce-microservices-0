using BuildingBlocks.Core.CQRS.Events.Internal;

namespace Services.Catalog.Products.Features.ChangingMaxThreshold;

public record MaxThresholdChanged(long ProductId, int MaxThreshold) : DomainEvent;
