using BuildingBlocks.Abstractions.CQRS.Commands;

namespace Services.Catalog.Products.Features.ChangingProductBrand;

internal record ChangeProductBrand : ITxCommand<ChangeProductBrandResult>;

internal record ChangeProductBrandResult;

internal class ChangeProductBrandHandler :
	ICommandHandler<ChangeProductBrand, ChangeProductBrandResult> {
	public Task<ChangeProductBrandResult> Handle(
		ChangeProductBrand command,
		CancellationToken cancellationToken) {
		return Task.FromResult<ChangeProductBrandResult>(null!);
	}
}
