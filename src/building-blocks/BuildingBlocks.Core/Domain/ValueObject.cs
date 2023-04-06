// Learn more:
// https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/microservice-ddd-cqrs-patterns/implement-value-objects
// https://enterprisecraftsmanship.com/posts/csharp-records-value-objects/
// https://enterprisecraftsmanship.com/posts/nulls-in-value-objects/
// https://enterprisecraftsmanship.com/posts/value-objects-when-to-create-one/
// https://blog.devgenius.io/3-different-ways-to-implement-value-object-in-csharp-10-d8f43e1fa4dc
// https://ardalis.com/working-with-value-objects/

namespace BuildingBlocks.Core.Domain;
public abstract class ValueObject : IEquatable<ValueObject> {
	protected abstract IEnumerable<Object> GetEqualityComponents();

	public Boolean Equals(ValueObject other) {
		return this.Equals(other as Object);
	}

	public override Boolean Equals(Object obj) {
		if(obj == null || obj.GetType() != this.GetType()) {
			return false;
		}

		ValueObject other = (ValueObject)obj;
		using IEnumerator<Object> thisValues = this.GetEqualityComponents().GetEnumerator();
		using IEnumerator<Object> otherValues = other.GetEqualityComponents().GetEnumerator();

		while(thisValues.MoveNext() && otherValues.MoveNext()) {
			if(thisValues.Current is null ^ otherValues.Current is null) {
				return false;
			}

			if(thisValues.Current?.Equals(otherValues.Current) is false) {
				return false;
			}
		}

		return thisValues.MoveNext() is false && otherValues.MoveNext() is false;
	}

	public override Int32 GetHashCode() {
		return this.GetEqualityComponents()
			.Select(x => x is not null ? x.GetHashCode() : default)
			.Aggregate((x, y) => x ^ y);
	}

	public static Boolean operator ==(ValueObject left, ValueObject right) {
		if(left is null && right is null) {
			return true;
		}

		return left is not null && right is not null && left.Equals(right);
	}

	public static Boolean operator !=(ValueObject left, ValueObject right) => !(left == right);
}
