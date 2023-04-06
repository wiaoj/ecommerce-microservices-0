using Ardalis.GuardClauses;
using ECommerce.Services.Catalogs.Suppliers.Exceptions.Application;

namespace Services.Catalog.Suppliers;

public static class GuardExtensions {
	public static void ExistsSupplier(this IGuardClause guardClause, bool exists, long supplierId) {
		if(exists == false)
			throw new SupplierCustomNotFoundException(supplierId);
	}
}
