namespace BuildingBlocks.Abstractions.Messaging.Context;

public sealed class ContextItems {
	private readonly Dictionary<String, Object?> items = new();

	public ContextItems AddItem(String key, Object? value) {
		this.items.TryAdd(key, value);
		return this;
	}

	public Type? TryGetItem<Type>(String key) {
		return this.items.TryGetValue(key, out var value) && value is Type type ? type : default;
	}
}