using MediatR;

namespace BuildingBlocks.Abstractions.Messaging.Message;
public interface IMessage : INotification {
	public Guid MessageId { get; }
	public DateTime Created { get; }
}