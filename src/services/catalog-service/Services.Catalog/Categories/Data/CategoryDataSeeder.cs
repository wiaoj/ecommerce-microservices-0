using BuildingBlocks.Abstractions.Persistence;
using Services.Catalog.Shared.Contracts;

namespace Services.Catalog.Categories.Data;
public class CategoryDataSeeder : IDataSeeder {
	private readonly ICatalogDbContext dbContext;

	public CategoryDataSeeder(ICatalogDbContext dbContext) {
		this.dbContext = dbContext;
	}

	public async Task SeedAllAsync() {
		if(await this.dbContext.Categories.AnyAsync()) {
			return;
		}

		await this.dbContext.Categories.AddRangeAsync(new List<Category> {
			Category.Create(Guid.NewGuid(), "Electronics", "0001", "All electronic goods"),
			Category.Create(Guid.NewGuid(), "Clothing", "0002", "All clothing goods"),
			Category.Create(Guid.NewGuid(), "Books", "0003", "All books"),
		});
		await this.dbContext.SaveChangesAsync(default);
	}

	public Int32 Order => 1;
}