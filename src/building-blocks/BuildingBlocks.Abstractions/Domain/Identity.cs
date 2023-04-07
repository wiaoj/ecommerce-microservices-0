namespace BuildingBlocks.Abstractions.Domain;
public abstract record Identity<TypeId> {
	public TypeId Value { get; private set; }

	protected Identity(TypeId value) {
		this.Value = value;
	}

	public static implicit operator TypeId(Identity<TypeId> identityId) => identityId.Value;

	public sealed override String ToString() {
		return this.IdAsString();
	}

	public String IdAsString() {
		return $"{this.GetType().Name} [Id={this.Value}]";
	}
}