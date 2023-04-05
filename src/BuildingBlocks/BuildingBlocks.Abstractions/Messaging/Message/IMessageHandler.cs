using BuildingBlocks.Abstractions.Messaging.Context;

namespace BuildingBlocks.Abstractions.Messaging.Message;

public interface IMessageHandler<in TypeMessage> where TypeMessage : class, IMessage {
	public Task HandleAsync(IConsumeContext<TypeMessage> messageContext, CancellationToken cancellationToken);
}