using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Persistence.EfCore.Postgres;
using BuildingBlocks.Persistence.Mongo;
using Services.Catalog.Shared.Contracts;
using Services.Catalog.Shared.Data;

namespace Services.Catalog.Shared.Extensions.WebApplicationBuilderExtensions;

public static partial class WebApplicationBuilderExtensions {
	public static WebApplicationBuilder AddStorage(this WebApplicationBuilder builder) {
		AddPostgresWriteStorage(builder.Services, builder.Configuration);
		AddMongoReadStorage(builder.Services, builder.Configuration);

		return builder;
	}

	private static void AddPostgresWriteStorage(IServiceCollection services, IConfiguration configuration) {
		if(configuration.GetValue<bool>("PostgresOptions:UseInMemory")) {
			services.AddDbContext<CatalogDbContext>(options =>
				options.UseInMemoryDatabase("ECommerce.Services.ECommerce.Services.Catalogs"));

			services.AddScoped<IDbFacadeResolver>(provider => provider.GetService<CatalogDbContext>()!);
		} else {
			services.AddPostgresDbContext<CatalogDbContext>(configuration);
		}

		services.AddScoped<ICatalogDbContext>(provider => provider.GetRequiredService<CatalogDbContext>());
	}

	private static void AddMongoReadStorage(IServiceCollection services, IConfiguration configuration) {
		services.AddMongoDbContext<CatalogReadDbContext>(configuration);
	}
}
