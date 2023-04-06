using BuildingBlocks.Persistence.Mongo;
using Humanizer;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Services.Catalog.Products.Models.Read;

namespace Services.Catalog.Shared.Data;

public class CatalogReadDbContext : MongoDbContext {
	public CatalogReadDbContext(IOptions<MongoOptions> options) : base(options.Value) {
		Products = GetCollection<ProductReadModel>(nameof(Products).Underscore());
	}

	public IMongoCollection<ProductReadModel> Products { get; }
}
