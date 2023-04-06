namespace BuildingBlocks.Abstractions.Domain;
public interface IEntity<out TypeId> : IHaveIdentity<TypeId>, IHaveCreator { }