using BuildingBlocks.Abstractions.Persistence.EfCore;
using Npgsql;
using System.Data;
using System.Data.Common;

namespace BuildingBlocks.Persistence.EfCore.Postgres;
public class NpgsqlConnectionFactory : IConnectionFactory {
	private readonly String connectionString;
	private DbConnection? connection;

	public NpgsqlConnectionFactory(String connectionString) {
		ArgumentException.ThrowIfNullOrEmpty(connectionString);
		this.connectionString = connectionString;
	}

	public async Task<IDbConnection> GetOrCreateConnectionAsync() {
		if(this.connection is null || this.connection.State is not ConnectionState.Open) {
			this.connection = new NpgsqlConnection(this.connectionString);
			await this.connection.OpenAsync();
		}

		return this.connection;
	}

	public void Dispose() {
		if(this.connection is { State: ConnectionState.Open }) {
			this.connection.Dispose();
		}
	}
}