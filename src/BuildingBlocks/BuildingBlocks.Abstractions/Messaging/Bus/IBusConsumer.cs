using BuildingBlocks.Abstractions.Messaging.Message;

namespace BuildingBlocks.Abstractions.Messaging.Bus;
public interface IBusConsumer {
	public void Consume<TypeMessage>(
		IMessageHandler<TypeMessage> handler,
		Action<IConsumeConfigurationBuilder>? consumeBuilder)
		where TypeMessage : class, IMessage;

	public Task Consume<TypeMessage>(
		MessageHandler<TypeMessage> subscribeMethod,
		Action<IConsumeConfigurationBuilder>? consumeBuilder,
		CancellationToken cancellationToken) where TypeMessage : class, IMessage;

	public Task Consume<TypeMessage>(CancellationToken cancellationToken) where TypeMessage : class, IMessage;

	public Task Consume(Type messageType, CancellationToken cancellationToken);

	public Task Consume<THandler, TypeMessage>(CancellationToken cancellationToken)
		where THandler : IMessageHandler<TypeMessage>
		where TypeMessage : class, IMessage;

	public Task ConsumeAll(CancellationToken cancellationToken);

	public Task ConsumeAllFromAssemblyOf<TType>(CancellationToken cancellationToken);

	public Task ConsumeAllFromAssemblyOf(CancellationToken cancellationToken, params Type[] assemblyMarkerTypes);
}