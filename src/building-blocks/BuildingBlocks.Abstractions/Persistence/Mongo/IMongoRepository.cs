using BuildingBlocks.Abstractions.Domain;

namespace BuildingBlocks.Abstractions.Persistence.Mongo;
public interface IMongoRepository<TypeEntity, in TypeId> : IRepository<TypeEntity, TypeId> where TypeEntity : class, IHaveIdentity<TypeId> { }