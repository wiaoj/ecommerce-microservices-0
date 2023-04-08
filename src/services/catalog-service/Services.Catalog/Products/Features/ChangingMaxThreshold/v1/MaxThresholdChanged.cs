using BuildingBlocks.Core.CQRS.Events.Internal;

namespace Services.Catalog.Products.Features.ChangingMaxThreshold.v1;
public record MaxThresholdChanged(Guid ProductId, int MaxThreshold) : DomainEvent;