using BuildingBlocks.Abstractions.Messaging.Context;
using BuildingBlocks.Abstractions.Messaging.Message;

public delegate Task MessageHandler<in TypeMessage>(
	IConsumeContext<TypeMessage> context,
	CancellationToken cancellationToken) where TypeMessage : class, IMessage;