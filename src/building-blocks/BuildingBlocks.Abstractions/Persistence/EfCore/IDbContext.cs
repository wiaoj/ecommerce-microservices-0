using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BuildingBlocks.Abstractions.Persistence.EfCore;
public interface IDbContext : ITxDbContextExecution, IRetryDbContextExecution {
	public DbSet<TypeEntity> Set<TypeEntity>() where TypeEntity : class;

	public Task BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken);
	public Task CommitTransactionAsync(CancellationToken cancellationToken);
	public Task RollbackTransactionAsync(CancellationToken cancellationToken);
	public Task<Boolean> SaveEntitiesAsync(CancellationToken cancellationToken);
	public Task<Int32> SaveChangesAsync(CancellationToken cancellationToken);
}