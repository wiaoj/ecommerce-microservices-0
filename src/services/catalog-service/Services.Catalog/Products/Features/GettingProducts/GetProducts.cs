using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Queries;
using BuildingBlocks.Core.CQRS.Queries;
using BuildingBlocks.Core.Persistence.EfCore;
using BuildingBlocks.Core.Types;
using Services.Catalog.Products.Dtos;
using Services.Catalog.Products.Models;
using Services.Catalog.Shared.Contracts;

namespace Services.Catalog.Products.Features.GettingProducts;

public record GetProducts : ListQuery<GetProductsResponse> {
	public class Validator : AbstractValidator<GetProducts> {
		public Validator() {
			CascadeMode = CascadeMode.Stop;

			RuleFor(x => x.Page)
				.GreaterThanOrEqualTo(1).WithMessage("Page should at least greater than or equal to 1.");

			RuleFor(x => x.PageSize)
				.GreaterThanOrEqualTo(1).WithMessage("PageSize should at least greater than or equal to 1.");
		}
	}

	public class Handler : IQueryHandler<GetProducts, GetProductsResponse> {
		private readonly ICatalogDbContext _catalogDbContext;
		private readonly IMapper _mapper;

		public Handler(IMapper mapper, ICatalogDbContext catalogDbContext) {
			_catalogDbContext = catalogDbContext;
			_mapper = mapper;
		}

		public async Task<GetProductsResponse> Handle(GetProducts request, CancellationToken cancellationToken) {
			var products = await _catalogDbContext.Products
				.OrderByDescending(x => x.Created)
				.ApplyIncludeList(request.Includes)
				.ApplyFilter(request.Filters)
				.AsNoTracking()
				.ApplyPagingAsync<Product, ProductDto>(
					_mapper.ConfigurationProvider,
					request.Page,
					request.PageSize,
					cancellationToken: cancellationToken);

			return new GetProductsResponse(products);
		}
	}
}
