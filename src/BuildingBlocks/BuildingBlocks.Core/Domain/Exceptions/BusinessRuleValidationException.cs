using BuildingBlocks.Abstractions.Domain;

namespace BuildingBlocks.Core.Domain.Exceptions;
public sealed class BusinessRuleValidationException : DomainException {
	public IBusinessRule BrokenRule { get; }
	public String Details { get; }

	public BusinessRuleValidationException(IBusinessRule brokenRule) : base(brokenRule.Message) {
		this.BrokenRule = brokenRule;
		this.Details = brokenRule.Message;
	}

	public sealed override String ToString() {
		return $"{this.BrokenRule.GetType().FullName}: {this.Details}";
	}
}