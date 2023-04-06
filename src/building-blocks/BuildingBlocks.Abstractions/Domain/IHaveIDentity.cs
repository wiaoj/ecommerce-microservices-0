namespace BuildingBlocks.Abstractions.Domain;
public interface IHaveIdentity<out TypeId> : IHaveIdentity {
	public new TypeId Id { get; }
	Object IHaveIdentity.Id => Id;
}

public interface IHaveIdentity {
	public Object Id { get; }
}