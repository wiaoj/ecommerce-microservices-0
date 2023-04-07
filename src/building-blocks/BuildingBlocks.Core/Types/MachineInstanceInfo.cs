using BuildingBlocks.Abstractions.Types;

namespace BuildingBlocks.Core.Types;
public record MachineInstanceInfo : IMachineInstanceInfo {
	public Guid ClientId { get; }
	public String ClientGroup { get; }

	public MachineInstanceInfo(Guid clientId, String clientGroup) {
		ArgumentException.ThrowIfNullOrEmpty(clientGroup, nameof(clientGroup));

		this.ClientId = clientId;
		this.ClientGroup = clientGroup;
	}

	internal static MachineInstanceInfo New() {
		return new(Guid.NewGuid(), AppDomain.CurrentDomain.FriendlyName);
	}
}