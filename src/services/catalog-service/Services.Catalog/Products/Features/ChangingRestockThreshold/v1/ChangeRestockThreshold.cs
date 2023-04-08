using BuildingBlocks.Abstractions.CQRS.Commands;

namespace Services.Catalog.Products.Features.ChangingRestockThreshold.v1;
public record ChangeRestockThreshold(Guid ProductId, Int32 NewRestockThreshold) : ITxCommand;