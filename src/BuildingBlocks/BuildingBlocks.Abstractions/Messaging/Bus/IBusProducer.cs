using BuildingBlocks.Abstractions.Messaging.Message;

namespace BuildingBlocks.Abstractions.Messaging.Bus;
public interface IBusProducer {
	public Task PublishAsync<TMessage>(
		TMessage message,
		IDictionary<String, Object?>? headers,
		CancellationToken cancellationToken) where TMessage : class, IMessage;

	public Task PublishAsync<TMessage>(
		TMessage message,
		IDictionary<String, Object?>? headers,
		String? exchangeOrTopic,
		String? queue,
		CancellationToken cancellationToken) where TMessage : class, IMessage;

	public Task PublishAsync(
		Object message,
		IDictionary<String, Object?>? eaders,
		CancellationToken cancellationToken);

	public Task PublishAsync(
		Object message,
		IDictionary<String, Object?>? headers,
		String? exchangeOrTopic,
		String? queue,
		CancellationToken cancellationToken);
}