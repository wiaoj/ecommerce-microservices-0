namespace BuildingBlocks.Core.Messaging;
public static class MessageHeaders {
	public const String MessageId = "message-id";
	public const String CorrelationId = "correlation-id";
	public const String CausationId = "causation-id";
	public const String TraceId = "trace-id";
	public const String SpanId = "span-id";
	public const String ParentSpanId = "parent-id";
	public const String Name = "name";
	public const String Type = "type";
	public const String Created = "created";
}