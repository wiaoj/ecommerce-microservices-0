using System.Data;

namespace BuildingBlocks.Abstractions.Persistence.EfCore;
public interface IConnectionFactory : IDisposable {
	public Task<IDbConnection> GetOrCreateConnectionAsync();
}