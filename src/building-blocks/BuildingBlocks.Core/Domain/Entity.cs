using BuildingBlocks.Abstractions.Domain;

namespace BuildingBlocks.Core.Domain;
public abstract class Entity<TypeId> : IEntity<TypeId> {
	public TypeId Id { get; protected init; } = default!;
	public DateTime Created { get; protected init; } = default!;
	public Int32? CreatedBy { get; protected init; } = default!;
}