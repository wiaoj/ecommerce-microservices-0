using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Abstractions.Web.Module;
using Services.Catalog.Suppliers.Data;

namespace Services.Catalog.Suppliers;

internal class Configs : IModuleConfiguration {
	public WebApplicationBuilder AddModuleServices(WebApplicationBuilder builder) {
		builder.Services.AddScoped<IDataSeeder, SupplierDataSeeder>();

		return builder;
	}

	public Task<WebApplication> ConfigureModule(WebApplication app) {
		return Task.FromResult(app);
	}

	public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) {
		return endpoints;
	}
}
