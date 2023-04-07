using BuildingBlocks.Abstractions.Domain;
using System.Linq.Expressions;

namespace BuildingBlocks.Abstractions.Persistence;
public interface IReadRepository<TypeEntity, in TypeId> where TypeEntity : class, IHaveIdentity<TypeId> {
	public Task<TypeEntity?> FindByIdAsync(TypeId id, CancellationToken cancellationToken);

	public Task<TypeEntity?> FindOneAsync(Expression<Func<TypeEntity, Boolean>> predicate, CancellationToken cancellationToken);

	public Task<IReadOnlyList<TypeEntity>> FindAsync(Expression<Func<TypeEntity, Boolean>> predicate, CancellationToken cancellationToken);

	public Task<IReadOnlyList<TypeEntity>> GetAllAsync(CancellationToken cancellationToken);
}

public interface IWriteRepository<TypeEntity, in TypeId> where TypeEntity : class, IHaveIdentity<TypeId> {
	public Task<TypeEntity> AddAsync(TypeEntity entity, CancellationToken cancellationToken);
	public Task<TypeEntity> UpdateAsync(TypeEntity entity, CancellationToken cancellationToken);
	public Task DeleteRangeAsync(IReadOnlyList<TypeEntity> entities, CancellationToken cancellationToken);
	public Task DeleteAsync(Expression<Func<TypeEntity, Boolean>> predicate, CancellationToken cancellationToken);
	public Task DeleteAsync(TypeEntity entity, CancellationToken cancellationToken);
	public Task DeleteByIdAsync(TypeId id, CancellationToken cancellationToken);
}

public interface IRepository<TypeEntity, in TypeId> : IReadRepository<TypeEntity, TypeId>, IWriteRepository<TypeEntity, TypeId>, IDisposable
	where TypeEntity : class, IHaveIdentity<TypeId> { }