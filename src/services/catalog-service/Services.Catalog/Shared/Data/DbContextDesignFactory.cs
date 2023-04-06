using BuildingBlocks.Persistence.EfCore.Postgres;

namespace Services.Catalog.Shared.Data;

public class CatalogDbContextDesignFactory : DbContextDesignFactoryBase<CatalogDbContext> {
	public CatalogDbContextDesignFactory() : base("ConnectionStrings:CatalogServiceConnection") {
	}
}
