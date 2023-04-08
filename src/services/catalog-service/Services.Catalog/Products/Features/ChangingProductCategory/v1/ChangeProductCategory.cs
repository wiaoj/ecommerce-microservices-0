using BuildingBlocks.Abstractions.CQRS.Commands;

namespace Services.Catalog.Products.Features.ChangingProductCategory.v1;
internal record ChangeProductCategory : ITxCommand<ChangeProductCategoryResult>;

internal record ChangeProductCategoryResult;

internal class ChangeProductCategoryHandler : ICommandHandler<ChangeProductCategory, ChangeProductCategoryResult> {
	public Task<ChangeProductCategoryResult> Handle(ChangeProductCategory command, CancellationToken cancellationToken) {
		return Task.FromResult<ChangeProductCategoryResult>(null!);
	}
}