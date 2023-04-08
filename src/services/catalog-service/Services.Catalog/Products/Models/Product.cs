using BuildingBlocks.Core.Domain;
using Services.Catalog.Products.Exceptions.Domain;
using Services.Catalog.Products.Features.CreatingProduct.v1.Events.Domain;
using Services.Catalog.Brands;
using Services.Catalog.Categories;
using Services.Catalog.Products.ValueObjects;
using Services.Catalog.Suppliers;
using Services.Catalog.Products.Features.ChangingProductPrice.v1;
using Services.Catalog.Products.Features.ChangingMaxThreshold.v1;
using Services.Catalog.Products.Features.ChangingRestockThreshold.v1;
using Services.Catalog.Products.Features.ReplenishingProductStock.v1.Events.Domain;

namespace Services.Catalog.Products.Models;

// https://event-driven.io/en/notes_about_csharp_records_and_nullable_reference_types/
// https://enterprisecraftsmanship.com/posts/link-to-an-aggregate-reference-or-id/
// https://ardalis.com/avoid-collections-as-properties/?utm_sq=grcpqjyka3
public class Product : Aggregate<ProductId> {
	private List<ProductImage> _images = new();

	public Name Name { get; private set; } = default!;
	public string? Description { get; private set; }
	public Price Price { get; private set; } = null!;
	public ProductColor Color { get; private set; }
	public ProductStatus ProductStatus { get; private set; }
	public Category? Category { get; private set; }
	public CategoryId CategoryId { get; private set; } = null!;
	public SupplierId SupplierId { get; private set; } = null!;
	public Supplier? Supplier { get; private set; } = null!;
	public BrandId BrandId { get; private set; } = null!;
	public Brand? Brand { get; private set; } = null!;
	public Size Size { get; private set; } = null!;
	public Stock Stock { get; set; } = null!;
	public Dimensions Dimensions { get; private set; } = null!;
	public IReadOnlyList<ProductImage> Images => _images;

	private Product(ProductId id, Stock stock) {
		if(id is null) {
			throw new ProductDomainException("Product id can not be null");
		}

		if(stock is null) {
			throw new ProductDomainException("Product stock can not be null");
		}

		this.Id = id;
		this.Stock = stock;
	}
	
	public static Product Create(
		ProductId id,
		Name name,
		Stock stock,
		ProductStatus status,
		Dimensions dimensions,
		Size size,
		ProductColor color,
		string? description,
		Price price,
		CategoryId categoryId,
		SupplierId supplierId,
		BrandId brandId,
		IList<ProductImage>? images = null) {
		var product = new Product(id, stock);

		product.ChangeName(name);
		product.ChangeSize(size);
		product.ChangeDescription(description);
		product.ChangePrice(price);
		product.AddProductImages(images);
		product.ChangeStatus(status);
		product.ChangeColor(color);
		product.ChangeDimensions(dimensions);
		product.ChangeCategory(categoryId);
		product.ChangeBrand(brandId);
		product.ChangeSupplier(supplierId);

		product.AddDomainEvents(new ProductCreated(product));

		return product;
	}

	public void ChangeStatus(ProductStatus status) {
		ProductStatus = status;
	}

	public void ChangeDimensions(Dimensions dimensions) {
		if(dimensions is null) {
			throw new ProductDomainException("Dimensions cannot be null.");
		}

		Dimensions = dimensions;
	}

	public void ChangeSize(Size size) {
		if(size is null) {
			throw new ProductDomainException("Size cannot be null.");
		}

		Size = size;
	}

	public void ChangeColor(ProductColor color) {
		Color = color;
	}

	/// <summary>
	/// Sets catalog item name.
	/// </summary>
	/// <param name="name">The name to be changed.</param>
	public void ChangeName(Name name) {
		if(name is null) {
			throw new ProductDomainException("Product name cannot be null.");
		}

		Name = name;
	}

	/// <summary>
	/// Sets catalog item description.
	/// </summary>
	/// <param name="description">The description to be changed.</param>
	public void ChangeDescription(string? description) {
		Description = description;
	}

	/// <summary>
	/// Sets catalog item price.
	/// </summary>
	/// <remarks>
	/// Raise a <see cref="ProductPriceChanged"/>.
	/// </remarks>
	/// <param name="price">The price to be changed.</param>
	public void ChangePrice(Price price) {
		if(price is null) {
			throw new ProductDomainException("Price cannot be null.");
		}

		if(Price == price)
			return;

		Price = price;

		AddDomainEvents(new ProductPriceChanged(price));
	}

	/// <summary>
	/// Decrements the quantity of a particular item in inventory and ensures the restockThreshold hasn't
	/// been breached. If so, a RestockRequest is generated in CheckThreshold.
	/// </summary>
	/// <param name="quantity">The number of items to debit.</param>
	/// <returns>int: Returns the number actually removed from stock. </returns>
	public int DebitStock(int quantity) {
		if(quantity < 0)
			quantity *= -1;

		if(HasStock(quantity) == false) {
			throw new InsufficientStockException(
				$"Empty stock, product item '{Name}' with quantity '{quantity}' is not available.");
		}

		int removed = Math.Min(quantity, Stock.Available);

		Stock = Stock.Create(Stock.Available - removed, Stock.RestockThreshold, Stock.MaxStockThreshold);

		if(Stock.Available <= Stock.RestockThreshold) {
			AddDomainEvents(new ProductRestockThresholdReachedEvent(Id, Stock, quantity));
		}

		AddDomainEvents(new ProductStockDebited(Id, Stock, quantity));

		return removed;
	}

	/// <summary>
	/// Increments the quantity of a particular item in inventory.
	/// </summary>
	/// <returns>int: Returns the quantity that has been added to stock.</returns>
	/// <param name="quantity">The number of items to Replenish.</param>
	public Stock ReplenishStock(int quantity) {
		// we don't have enough space in the inventory
		if(Stock.Available + quantity > Stock.MaxStockThreshold) {
			throw new MaxStockThresholdReachedException(
				$"Max stock threshold has been reached. Max stock threshold is {Stock.MaxStockThreshold}");
		}

		Stock = Stock.Create(Stock.Available + quantity, Stock.RestockThreshold, Stock.MaxStockThreshold);

		AddDomainEvents(new ProductStockReplenished(Id, Stock, quantity));

		return Stock;
	}

	public Stock ChangeMaxStockThreshold(int maxStockThreshold) {
		if(maxStockThreshold <= default(Int32)) {
			throw new ArgumentException(null, nameof(maxStockThreshold));
		}

		Stock = Stock.Create(Stock.Available, Stock.RestockThreshold, maxStockThreshold);

		AddDomainEvents(new MaxThresholdChanged(Id, maxStockThreshold));

		return Stock;
	}

	public Stock ChangeRestockThreshold(int restockThreshold) {
		if(restockThreshold <= default(Int32)) {
			throw new ArgumentException(null, nameof(restockThreshold));
		}

		Stock = Stock.Create(Stock.Available, restockThreshold, Stock.MaxStockThreshold);

		AddDomainEvents(new RestockThresholdChanged(Id, restockThreshold));

		return Stock;
	}

	public bool HasStock(int quantity) {
		return Stock.Available >= quantity;
	}

	public void Activate() => ChangeStatus(ProductStatus.Available);

	public void DeActive() => ChangeStatus(ProductStatus.Unavailable);

	/// <summary>
	/// Sets category.
	/// </summary>
	/// <param name="categoryId">The categoryId to be changed.</param>
	public void ChangeCategory(CategoryId categoryId) {
		if(categoryId is null) {
			throw new ProductDomainException("CategoryId cannot be null");
		}

		// raising domain event immediately for checking some validation rule with some dependencies such as database
		DomainEventsInvoker.RaiseDomainEvent(new ChangingProductCategory(categoryId));

		CategoryId = categoryId;

		// add event to domain events list that will be raise during commiting transaction
		AddDomainEvents(new ProductCategoryChanged(categoryId, Id));
	}

	/// <summary>
	/// Sets supplier.
	/// </summary>
	/// <param name="supplierId">The supplierId to be changed.</param>
	public void ChangeSupplier(SupplierId supplierId) {
		if(supplierId is null) {
			throw new ProductDomainException("SupplierId cannot be null");
		}

		DomainEventsInvoker.RaiseDomainEvent(new ChangingProductSupplier(supplierId));

		SupplierId = supplierId;

		AddDomainEvents(new ProductSupplierChanged(supplierId, Id));
	}

	/// <summary>
	///  Sets brand.
	/// </summary>
	/// <param name="brandId">The brandId to be changed.</param>
	public void ChangeBrand(BrandId brandId) {
		Guard.Against.Null(brandId, new ProductDomainException("brandId cannot be null"));

		DomainEventsInvoker.RaiseDomainEvent(new ChangingProductBrand(brandId));

		BrandId = brandId;

		AddDomainEvents(new ProductBrandChanged(brandId, Id));
	}

	public void AddProductImages(IList<ProductImage>? productImages) {
		if(productImages is null) {
			_images = null!;
			return;
		}

		_images.AddRange(productImages);
	}
}
