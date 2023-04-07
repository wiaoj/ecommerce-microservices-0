using BuildingBlocks.Abstractions.Messaging.Message;

namespace BuildingBlocks.Abstractions.Serialization;
public interface IMessageSerializer : ISerializer {
	/// <summary>
	/// Serializes the given <see cref="MessageEnvelope"/> into a string
	/// </summary>
	/// <param name="messageEnvelope">a messageEnvelope that implement IMessage interface.</param>
	/// <returns>a json string for serialized messageEnvelope.</returns>
	public String Serialize(MessageEnvelope messageEnvelope);

	public String Serialize<TypeMessage>(TypeMessage message) where TypeMessage : IMessage;

	/// <summary>
	/// Deserialize the given string into a <see cref="MessageEnvelope"/>
	/// </summary>
	/// <param name="json">a json data to deserialize to a messageEnvelope.</param>
	/// <returns>return a messageEnvelope type.</returns>
	public MessageEnvelope? Deserialize(String json);

	/// <summary>
	/// Deserialize the given byte array back into a message.
	/// </summary>
	/// <param name="data"></param>
	/// <param name="payloadType"></param>
	/// <returns></returns>
	public IMessage? Deserialize(ReadOnlySpan<Byte> data, String payloadType);

	/// <summary>
	///  Deserialize the given string into a <see cref="TMessage"/>.
	/// </summary>
	/// <param name="message"></param>
	/// <typeparam name="TypeMessage"></typeparam>
	/// <returns></returns>
	public TypeMessage? Deserialize<TypeMessage>(String message) where TypeMessage : IMessage;

	/// <summary>
	/// Deserialize the given string into a object.
	/// </summary>
	/// <param name="payload"></param>
	/// <param name="payloadType"></param>
	/// <returns></returns>
	public Object? Deserialize(String payload, String payloadType);
}