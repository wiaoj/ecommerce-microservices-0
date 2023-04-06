using Ardalis.GuardClauses;
using ECommerce.Services.Catalogs.Categories.Exceptions.Application;

namespace Services.Catalog.Categories;

public static class GuardExtensions {
	public static void ExistsCategory(this IGuardClause guardClause, bool exists, long categoryId) {
		if(exists == false)
			throw new CategoryCustomNotFoundException(categoryId);
	}
}
