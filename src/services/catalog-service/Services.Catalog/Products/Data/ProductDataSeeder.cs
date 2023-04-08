using Bogus;
using BuildingBlocks.Abstractions.Persistence;
using Services.Catalog.Products.Models;
using Services.Catalog.Products.ValueObjects;
using Services.Catalog.Shared.Contracts;

namespace Services.Catalog.Products.Data;
public class ProductDataSeeder : IDataSeeder {
	private readonly ICatalogDbContext dbContext;

	public ProductDataSeeder(ICatalogDbContext dbContext) {
		this.dbContext = dbContext;
	}

	public async Task SeedAllAsync() {
		if(await dbContext.Products.AnyAsync())
			return;


		// https://github.com/bchavez/Bogus
		// https://www.youtube.com/watch?v=T9pwE1GAr_U
		var productFaker = new Faker<Product>().CustomInstantiator(faker => {
			var brand = Product.Create(
				Guid.NewGuid(),
				faker.Commerce.ProductName(),
				Stock.Create(faker.Random.Int(10, 20), 5, 20),
				ProductStatus.Available,
				Dimensions.Create(faker.Random.Int(10, 50), faker.Random.Int(10, 50), faker.Random.Int(10, 50)),
				faker.PickRandom<string>("M", "S", "L"),
				faker.Random.Enum<ProductColor>(),
				faker.Commerce.ProductDescription(),
				Price.Create(faker.PickRandom<decimal>(100, 200, 500)),
				faker.Random.Long(1, 3),
				faker.Random.Long(1, 5),
				faker.Random.Long(1, 5));

			return brand;
		});
		var products = productFaker.Generate(5);

		await dbContext.Products.AddRangeAsync(products);
		await dbContext.SaveChangesAsync();
	}

	public int Order => 4;
}
