namespace BuildingBlocks.Core.Web;
public class AppOptions {
	public String? Name { get; set; }
	public String? ApiAddress { get; set; }
	public String? Instance { get; set; }
	public String? Version { get; set; }
	public Boolean DisplayBanner { get; set; } = true;
	public Boolean DisplayVersion { get; set; } = true;
	public String? Description { get; set; }
}