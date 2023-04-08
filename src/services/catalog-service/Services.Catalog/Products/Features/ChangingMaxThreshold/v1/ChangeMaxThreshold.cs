using BuildingBlocks.Abstractions.CQRS.Commands;

namespace Services.Catalog.Products.Features.ChangingMaxThreshold.v1;
public record ChangeMaxThreshold(Guid ProductId, int NewMaxThreshold) : ITxCommand;