namespace BuildingBlocks.Abstractions.Messaging.PersistMessage;
public class StoreMessage {
	public Guid Id { get; private set; }
	public String DataType { get; private set; }
	public String Data { get; private set; }
	public DateTime Created { get; private set; }
	public Int32 RetryCount { get; private set; }
	public MessageStatus MessageStatus { get; private set; }
	public MessageDeliveryType DeliveryType { get; private set; }

	public StoreMessage(Guid id, String dataType, String data, MessageDeliveryType deliveryType) {
		this.Id = id;
		this.DataType = dataType;
		this.Data = data;
		this.DeliveryType = deliveryType;
		this.Created = DateTime.Now;
		this.MessageStatus = MessageStatus.Stored;
		this.RetryCount = default;
	}

	public void ChangeState(MessageStatus messageStatus) {
		this.MessageStatus = messageStatus;
	}

	public void IncreaseRetry() {
		this.RetryCount++;
	}
}