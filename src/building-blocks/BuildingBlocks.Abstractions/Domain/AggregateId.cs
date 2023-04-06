namespace BuildingBlocks.Abstractions.Domain;
public record AggregateId<T> : Identity<T> {
	public AggregateId(T value) : base(value) {
	}

	public static implicit operator T(AggregateId<T> id) {
		ArgumentNullException.ThrowIfNull(id.Value, nameof(id.Value));
		return id;
	}

	public static implicit operator AggregateId<T>(T id) => new(id);
}

public record AggregateId : AggregateId<Guid> {
	public AggregateId(Guid value) : base(value) { }


	public static implicit operator Guid(AggregateId id) {
		ArgumentNullException.ThrowIfNull(id.Value, nameof(id.Value));
		return id;
	}

	public static implicit operator AggregateId(Guid id) => new(id);
}
