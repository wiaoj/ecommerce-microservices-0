using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Abstractions.Web.Module;
using Services.Catalog.Brands.Data;

namespace Services.Catalog.Brands;
internal class Configs : IModuleConfiguration {
	public WebApplicationBuilder AddModuleServices(WebApplicationBuilder builder) {
		builder.Services.AddScoped<IDataSeeder, BrandDataSeeder>();

		return builder;
	}

	public Task<WebApplication> ConfigureModule(WebApplication app) {
		return Task.FromResult(app);
	}

	public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) {
		return endpoints;
	}
}