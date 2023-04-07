using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BuildingBlocks.Abstractions.Persistence;
public interface IDbFacadeResolver {
	public DatabaseFacade Database { get; }
}