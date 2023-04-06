namespace BuildingBlocks.Abstractions.Persistence.Mongo;
public interface IMongoUnitOfWork<out TypeContext> : IUnitOfWork<TypeContext> where TypeContext : class, IMongoDbContext { }