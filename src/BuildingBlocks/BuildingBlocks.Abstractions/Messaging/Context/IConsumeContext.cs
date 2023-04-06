using BuildingBlocks.Abstractions.Messaging.Message;
using System.Diagnostics;

namespace BuildingBlocks.Abstractions.Messaging.Context;
public interface IConsumeContext<out TypeMessage> : IConsumeContext where TypeMessage : class, IMessage {
	public new TypeMessage Message { get; }
}
public interface IConsumeContext {
	public Guid MessageId { get; }
	public String MessageType { get; }
	public Object Message { get; }
	public IDictionary<String, Object?> Headers { get; }
	public ActivityContext? ParentContext { get; set; }
	public ContextItems Items { get; }
	public Int32 PayloadSize { get; }
	public UInt64 Version { get; }
	public DateTime Created { get; }
}