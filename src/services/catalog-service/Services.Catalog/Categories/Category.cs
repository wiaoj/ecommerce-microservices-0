using BuildingBlocks.Core.Domain;
using Services.Catalog.Categories.Exceptions.Domain;

namespace Services.Catalog.Categories;
public class Category : Aggregate<CategoryId> {
	public String Name { get; private set; }
	public String Description { get; private set; }
	public String Code { get; private set; }

	private Category(CategoryId id) {
		this.Id = id;
	}

	public static Category Create(CategoryId id, String name, String code) {
		return Create(id, name, code, String.Empty);
	}

	public static Category Create(CategoryId id, String name, String code, String description) {
		ArgumentNullException.ThrowIfNull(id, nameof(id));
		Category category = new(id);

		category.ChangeName(name);
		category.ChangeDescription(description);
		category.ChangeCode(code);

		return category;
	}

	public void ChangeName(String name) {
		if(String.IsNullOrWhiteSpace(name)) {
			throw new CategoryDomainException("Name can't be white space or null.");
		}

		this.Name = name;
	}

	public void ChangeCode(String code) {
		if(String.IsNullOrWhiteSpace(code)) {
			throw new CategoryDomainException("Code can't be white space or null.");
		}

		this.Code = code;
	}

	public void ChangeDescription(String description) {
		if(String.IsNullOrWhiteSpace(description)) {
			throw new CategoryDomainException("Description can't be white space or null.");
		}

		this.Description = description;
	}

	public override String ToString() {
		return $"{this.Name} - {this.Code}";
	}
}