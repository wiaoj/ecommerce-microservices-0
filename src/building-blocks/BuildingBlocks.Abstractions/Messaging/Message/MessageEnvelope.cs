// Ref: https://www.enterpriseintegrationpatterns.com/patterns/messaging/EnvelopeWrapper.html

namespace BuildingBlocks.Abstractions.Messaging.Message;
public class MessageEnvelope {
	public Object? Message { get; init; }
	public IDictionary<String, Object?> Headers { get; init; }

	public MessageEnvelope(Object? message, IDictionary<String, Object?>? headers) {
		this.Message = message;
		this.Headers = headers ?? new Dictionary<String, Object?>();
	}
}

public class MessageEnvelope<TypeMessage> : MessageEnvelope where TypeMessage : class, IMessage {
	public new TypeMessage? Message { get; }

	public MessageEnvelope(TypeMessage message, IDictionary<String, Object?>? headers) : base(message, headers) {
		this.Message = message;
	}
}