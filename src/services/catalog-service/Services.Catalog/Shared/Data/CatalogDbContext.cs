using BuildingBlocks.Core.Persistence.EfCore;
using Services.Catalog.Brands;
using Services.Catalog.Categories;
using Services.Catalog.Products.Models;
using Services.Catalog.Shared.Contracts;
using Services.Catalog.Suppliers;

namespace Services.Catalog.Shared.Data;

public class CatalogDbContext : EfDbContextBase, ICatalogDbContext {
	public const string DefaultSchema = "catalog";

	public CatalogDbContext(DbContextOptions options) : base(options) {
	}
	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

		base.OnModelCreating(modelBuilder);
	}

	public DbSet<Product> Products => Set<Product>();
	public DbSet<ProductView> ProductsView => Set<ProductView>();
	public DbSet<Category> Categories => Set<Category>();
	public DbSet<Supplier> Suppliers => Set<Supplier>();
	public DbSet<Brand> Brands => Set<Brand>();
}
