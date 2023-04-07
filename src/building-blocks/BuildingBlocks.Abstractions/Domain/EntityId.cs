namespace BuildingBlocks.Abstractions.Domain;
public record EntityId<T> : Identity<T> {
	public EntityId(T value) : base(value) { }

	public static implicit operator T(EntityId<T> id) {
		ArgumentNullException.ThrowIfNull(id.Value, nameof(id.Value));
		return id.Value;
	}

	public static EntityId<T> CreateEntityId(T id) {
		return new(id);
	}
}

public record EntityId : EntityId<Guid> {
	public EntityId(Guid value) : base(value) { }

	public static implicit operator Guid(EntityId id) {
		ArgumentNullException.ThrowIfNull(id.Value, nameof(id.Value));
		return id.Value;
	}

	public static new EntityId CreateEntityId(Guid id) {
		return new(id);
	}
}
