using BuildingBlocks.Abstractions.Messaging.PersistMessage;
using System.Linq.Expressions;

namespace BuildingBlocks.Core.Messaging.MessagePersistence;
public class NullPersistenceRepository : IMessagePersistenceRepository {
	public Task AddAsync(StoreMessage storeMessage, CancellationToken cancellationToken) {
		return Task.CompletedTask;
	}

	public Task UpdateAsync(StoreMessage storeMessage, CancellationToken cancellationToken) {
		return Task.CompletedTask;
	}

	public Task ChangeStateAsync(Guid messageId, MessageStatus status, CancellationToken cancellationToken) {
		return Task.CompletedTask;
	}

	public Task<IReadOnlyList<StoreMessage>> GetAllAsync(CancellationToken cancellationToken) {
		return new Task<IReadOnlyList<StoreMessage>>(null);
	}

	public Task<IReadOnlyList<StoreMessage>> GetByFilterAsync(
		Expression<Func<StoreMessage, Boolean>> predicate,
		CancellationToken cancellationToken
	) {
		return new Task<IReadOnlyList<StoreMessage>>(null);
	}

	public Task<StoreMessage?> GetByIdAsync(Guid id, CancellationToken cancellationToken) {
		return new Task<StoreMessage?>(null);
	}

	public Task<Boolean> RemoveAsync(StoreMessage storeMessage, CancellationToken cancellationToken) {
		return Task.FromResult(true);
	}

	public Task CleanupMessages() {
		return Task.CompletedTask;
	}
}