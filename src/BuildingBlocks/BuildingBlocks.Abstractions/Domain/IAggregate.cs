namespace BuildingBlocks.Abstractions.Domain;
public interface IAggregate<out TypeId> : IEntity<TypeId>, IHaveAggregate { }