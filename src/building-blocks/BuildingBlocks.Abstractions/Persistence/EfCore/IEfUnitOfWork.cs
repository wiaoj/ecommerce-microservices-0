using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Abstractions.Persistence.EfCore;
public interface IEfUnitOfWork : IUnitOfWork, ITransactionAble, ITxDbContextExecution, IRetryDbContextExecution { }

/// <summary>
/// Defines the interface(s) for generic unit of work.
/// </summary>
public interface IEfUnitOfWork<out TypeContext> : IEfUnitOfWork where TypeContext : DbContext {
	public TypeContext DbContext { get; }
}