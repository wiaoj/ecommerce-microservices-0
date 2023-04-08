using BuildingBlocks.Core.Domain;

namespace Services.Catalog.Suppliers;
public class Supplier : Entity<SupplierId> {
	public String Name { get; private set; }

	public Supplier(SupplierId id, String name) {
		this.Name = name;
		this.Id = id;
	}
}