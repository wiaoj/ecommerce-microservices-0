namespace BuildingBlocks.Abstractions.Domain;
public interface IBusinessRule {
	public Boolean IsBroken { get; }
	public String Message { get; }
}