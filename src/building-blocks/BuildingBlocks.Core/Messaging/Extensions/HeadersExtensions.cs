using BuildingBlocks.Core.Types.Extensions;

namespace BuildingBlocks.Core.Messaging.Extensions;

public static class HeadersExtensions {
	public static void AddCorrelationId(this IDictionary<String, Object?> header, String correlationId) {
		header.Add(MessageHeaders.CorrelationId, correlationId);
	}

	public static Guid? GetCorrelationId(this IDictionary<String, Object?> header) {
		String? id = header.Get<String>(MessageHeaders.CorrelationId);
		Guid.TryParse(id, out Guid result);

		return result;
	}

	public static void AddMessageName(this IDictionary<String, Object?> header, String messageName) {
		header.Add(MessageHeaders.Name, messageName);
	}

	public static String? GetMessageName(this IDictionary<String, Object?> header) {
		return header.Get<String>(MessageHeaders.Name);
	}

	public static String? GetMessageType(this IDictionary<String, Object?> header) {
		return header.Get<String>(MessageHeaders.Type);
	}

	public static void AddMessageType(this IDictionary<String, Object?> header, String messageType) {
		header.Add(MessageHeaders.Type, messageType);
	}

	public static void AddMessageId(this IDictionary<String, Object?> header, String messageId) {
		header.Add(MessageHeaders.MessageId, messageId);
	}

	public static Guid? GetMessageId(this IDictionary<String, Object?> header) {
		String? id = header.Get<String>(MessageHeaders.MessageId);
		Guid.TryParse(id, out Guid result);

		return result;
	}

	public static void AddCausationId(this IDictionary<String, Object?> header, String causationId) {
		header.Add(MessageHeaders.CausationId, causationId);
	}

	public static String? GetCausationId(this IDictionary<String, Object?> header) {
		return header.Get<String>(MessageHeaders.CorrelationId);
	}

	public static Int64? GetCreatedUnixTime(this IDictionary<String, Object?> header) {
		return header.Get(MessageHeaders.Created) as Int64?;
	}

	public static void AddCreatedUnixTime(this IDictionary<String, Object?> header, Int64 created) {
		header.Add(MessageHeaders.Created, created);
	}
}