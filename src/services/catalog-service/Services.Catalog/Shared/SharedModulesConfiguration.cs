using Asp.Versioning;
using Asp.Versioning.Builder;
using BuildingBlocks.Abstractions.Web.Module;
using BuildingBlocks.Core;
using BuildingBlocks.Monitoring;
using ECommerce.Services.Catalogs.Shared.Extensions.ApplicationBuilderExtensions;
using Services.Catalog.Shared.Extensions.WebApplicationBuilderExtensions;

namespace Services.Catalog.Shared;

public class SharedModulesConfiguration : ISharedModulesConfiguration {
	public const string CatalogModulePrefixUri = "api/v{version:apiVersion}/catalogs";

	public IEndpointRouteBuilder MapSharedModuleEndpoints(IEndpointRouteBuilder endpoints) {
		endpoints.MapGet("/", (HttpContext context) => {
			var requestId = context.Request.Headers.TryGetValue("X-Request-Id", out var requestIdHeader)
				? requestIdHeader.FirstOrDefault()
				: string.Empty;

			return $"Catalogs Service Apis, RequestId: {requestId}";
		}).ExcludeFromDescription();

		return endpoints;
	}

	public WebApplicationBuilder AddSharedModuleServices(WebApplicationBuilder builder) {
		builder.AddInfrastructure();

		builder.AddStorage();

		return builder;
	}

	public async Task<WebApplication> ConfigureSharedModule(WebApplication app) {
		ServiceActivator.Configure(app.Services);

		if(app.Environment.IsEnvironment("test") == false)
			app.UseMonitoring();

		await app.ApplyDatabaseMigrations(app.Logger);
		await app.SeedData(app.Logger, app.Environment);

		await app.UseInfrastructure(app.Logger);

		return app;
	}
}
