using BuildingBlocks.Abstractions.CQRS.Commands;

namespace Services.Catalog.Products.Features.ChangingMaxThreshold;

public record ChangeMaxThreshold(long ProductId, int NewMaxThreshold) : ITxCommand;
