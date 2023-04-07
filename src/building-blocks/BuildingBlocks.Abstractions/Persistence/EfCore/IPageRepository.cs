using BuildingBlocks.Abstractions.Domain;

namespace BuildingBlocks.Abstractions.Persistence.EfCore;
public interface IPageRepository<TypeEntity, TKey> where TypeEntity : IHaveIdentity<TKey> { }

public interface IPageRepository<TypeEntity> : IPageRepository<TypeEntity, Guid> where TypeEntity : IHaveIdentity<Guid> { }