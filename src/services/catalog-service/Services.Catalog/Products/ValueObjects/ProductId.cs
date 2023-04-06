using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.Domain;

namespace Services.Catalog.Products.ValueObjects;

public record ProductId : AggregateId {
	public ProductId(long value) : base(value) {
		Guard.Against.NegativeOrZero(value, nameof(value));
	}

	public static implicit operator long(ProductId id) => id.Value;

	public static implicit operator ProductId(long id) => new(id);
}
