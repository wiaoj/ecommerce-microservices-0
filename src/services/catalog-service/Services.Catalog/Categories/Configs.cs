using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Abstractions.Web.Module;
using Services.Catalog.Categories.Data;

namespace Services.Catalog.Categories;

internal class Configs : IModuleConfiguration {
	public WebApplicationBuilder AddModuleServices(WebApplicationBuilder builder) {
		builder.Services.AddScoped<IDataSeeder, CategoryDataSeeder>();

		return builder;
	}

	public Task<WebApplication> ConfigureModule(WebApplication app) {
		return Task.FromResult(app);
	}

	public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) {
		return endpoints;
	}
}
