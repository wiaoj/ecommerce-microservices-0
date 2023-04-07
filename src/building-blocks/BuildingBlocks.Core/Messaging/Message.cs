using BuildingBlocks.Abstractions.Messaging.Message;

namespace BuildingBlocks.Core.Messaging;
public record Message : IMessage {
	public Guid MessageId => Guid.NewGuid();
	public DateTime Created { get; } = DateTime.UtcNow;
}