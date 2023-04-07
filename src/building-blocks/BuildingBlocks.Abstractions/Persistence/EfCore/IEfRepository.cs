using BuildingBlocks.Abstractions.Domain;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace BuildingBlocks.Abstractions.Persistence.EfCore;
public interface IEfRepository<TypeEntity, in TypeId> : IRepository<TypeEntity, TypeId> where TypeEntity : class, IHaveIdentity<TypeId> {
	public IEnumerable<TypeEntity> GetInclude(Func<IQueryable<TypeEntity>, IIncludableQueryable<TypeEntity, Object>>? includes = null);

	public IEnumerable<TypeEntity> GetInclude(
		Expression<Func<TypeEntity, Boolean>> predicate,
		Func<IQueryable<TypeEntity>, IIncludableQueryable<TypeEntity, Object>>? includes = null,
		Boolean withTracking = true);

	public Task<IEnumerable<TypeEntity>> GetIncludeAsync(
		Func<IQueryable<TypeEntity>, IIncludableQueryable<TypeEntity, Object>>? includes = null);

	public Task<IEnumerable<TypeEntity>> GetIncludeAsync(
		Expression<Func<TypeEntity, Boolean>> predicate,
		Func<IQueryable<TypeEntity>, IIncludableQueryable<TypeEntity, Object>>? includes = null,
		Boolean withTracking = true);
}