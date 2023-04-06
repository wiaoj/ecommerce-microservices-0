using Asp.Versioning.Builder;
using BuildingBlocks.Abstractions.CQRS.Events;
using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Abstractions.Web.Module;
using Services.Catalog.Products.Data;
using Services.Catalog.Products.Features.CreatingProduct;
using Services.Catalog.Products.Features.DebitingProductStock;
using Services.Catalog.Products.Features.GettingProductById;
using Services.Catalog.Products.Features.GettingProductsView;
using Services.Catalog.Products.Features.ReplenishingProductStock;
using Services.Catalog.Products.Features.UpdatingProduct;
using Services.Catalog.Shared;

namespace Services.Catalog.Products;

internal class ProductsConfigs : IModuleConfiguration {
	public const string Tag = "Products";
	public const string ProductsPrefixUri = $"{SharedModulesConfiguration.CatalogModulePrefixUri}/products";
	public static ApiVersionSet VersionSet { get; private set; } = default!;

	public WebApplicationBuilder AddModuleServices(WebApplicationBuilder builder) {
		builder.Services.AddScoped<IDataSeeder, ProductDataSeeder>();
		builder.Services.AddSingleton<IEventMapper, ProductEventMapper>();

		return builder;
	}

	public Task<WebApplication> ConfigureModule(WebApplication app) {
		return Task.FromResult(app);
	}

	public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) {
		VersionSet = endpoints.NewApiVersionSet(Tag).Build();

		return endpoints.MapCreateProductsEndpoint()
			.MapUpdateProductEndpoint()
			.MapDebitProductStockEndpoint()
			.MapReplenishProductStockEndpoint()
			.MapGetProductByIdEndpoint()
			.MapGetProductsViewEndpoint();
	}
}
