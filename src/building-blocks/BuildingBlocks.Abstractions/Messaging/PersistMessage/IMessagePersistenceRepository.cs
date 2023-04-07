using System.Linq.Expressions;

namespace BuildingBlocks.Abstractions.Messaging.PersistMessage;
public interface IMessagePersistenceRepository {
	public Task AddAsync(StoreMessage storeMessage, CancellationToken cancellationToken);
	public Task UpdateAsync(StoreMessage storeMessage, CancellationToken cancellationToken);

	public Task ChangeStateAsync(Guid messageId, MessageStatus status, CancellationToken cancellationToken);

	public Task<IReadOnlyList<StoreMessage>> GetAllAsync(CancellationToken cancellationToken);

	public Task<IReadOnlyList<StoreMessage>> GetByFilterAsync(
		Expression<Func<StoreMessage, Boolean>> predicate,
		CancellationToken cancellationToken);

	public Task<StoreMessage?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

	public Task<Boolean> RemoveAsync(StoreMessage storeMessage, CancellationToken cancellationToken);

	public Task CleanupMessages();
}