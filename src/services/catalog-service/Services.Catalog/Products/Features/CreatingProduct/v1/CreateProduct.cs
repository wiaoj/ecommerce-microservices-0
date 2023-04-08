using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Domain;
using BuildingBlocks.Core.Exceptions;
using Services.Catalog.Brands;
using Services.Catalog.Brands.Exceptions.Application;
using Services.Catalog.Categories;
using Services.Catalog.Categories.Exceptions.Domain;
using Services.Catalog.Products.Dtos.v1;
using Services.Catalog.Products.Models;
using Services.Catalog.Products.ValueObjects;
using Services.Catalog.Shared.Contracts;
using Services.Catalog.Shared.Extensions;
using Services.Catalog.Suppliers;
using Services.Catalog.Suppliers.Exceptions.Application;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Services.Catalog.Products.Features.CreatingProduct.v1.Requests;
using Services.Catalog.Products.Models;
using Services.Catalog.Products.Dtos;

namespace Services.Catalog.Products.Features.CreatingProduct.v1;
public record CreateProduct(
	string Name,
	decimal Price,
	int Stock,
	int RestockThreshold,
	int MaxStockThreshold,
	ProductStatus Status,
	int Width,
	int Height,
	int Depth,
	string Size,
	ProductColor Color,
	Guid CategoryId,
	Guid SupplierId,
	Guid BrandId,
	string? Description = null,
	IEnumerable<CreateProductImageRequest>? Images = null
) : ITxCreateCommand<CreateProductResponse> {
	public Guid Id { get; init; } = Guid.NewGuid();
}

public class CreateProductValidator : AbstractValidator<CreateProduct> {
	public CreateProductValidator() {
		CascadeMode = CascadeMode.Stop;

		RuleFor(x => x.Id).NotEmpty().WithMessage("InternalCommandId must be greater than 0");

		RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");

		RuleFor(x => x.Price).NotEmpty().GreaterThan(0).WithMessage("Price must be greater than 0");

		RuleFor(x => x.Status).IsInEnum().WithMessage("Status is required.");

		RuleFor(x => x.Color).IsInEnum().WithMessage("Color is required.");

		RuleFor(x => x.Stock).NotEmpty().GreaterThan(0).WithMessage("Stock must be greater than 0");

		RuleFor(x => x.MaxStockThreshold)
			.NotEmpty()
			.GreaterThan(0)
			.WithMessage("MaxStockThreshold must be greater than 0");

		RuleFor(x => x.RestockThreshold)
			.NotEmpty()
			.GreaterThan(0)
			.WithMessage("RestockThreshold must be greater than 0");

		RuleFor(x => x.CategoryId).NotEmpty().WithMessage("CategoryId must be greater than 0");

		RuleFor(x => x.SupplierId).NotEmpty().WithMessage("SupplierId must be greater than 0");

		RuleFor(x => x.BrandId).NotEmpty().WithMessage("BrandId must be greater than 0");
	}
}

public class CreateProductHandler : ICommandHandler<CreateProduct, CreateProductResponse> {
	private readonly ILogger<CreateProductHandler> logger;
	private readonly IMapper mapper;
	private readonly ICatalogDbContext catalogDbContext;

	public CreateProductHandler(
		ICatalogDbContext catalogDbContext,
		IMapper mapper,
		ILogger<CreateProductHandler> logger
	) {
		ArgumentNullException.ThrowIfNull(logger, nameof(logger));
		ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));
		ArgumentNullException.ThrowIfNull(catalogDbContext, nameof(catalogDbContext));
		this.logger = logger;
		this.mapper = mapper;
		this.catalogDbContext = catalogDbContext;
	}

	public async Task<CreateProductResponse> Handle(CreateProduct command, CancellationToken cancellationToken) {
		ArgumentNullException.ThrowIfNull(command, nameof(command));

		var images = command.Images
			?.Select(
				x =>
					new ProductImage(
						EntityId.CreateEntityId(Guid.NewGuid()),
						x.ImageUrl,
						x.IsMain,
						ProductId.Of(command.Id)
					)
			)
			.ToList();

		var category = await catalogDbContext.FindCategoryAsync(CategoryId.Of(command.CategoryId));
		Guard.Against.NotFound(category, new CategoryDomainException(command.CategoryId));

		var brand = await catalogDbContext.FindBrandAsync(BrandId.Of(command.BrandId));
		Guard.Against.NotFound(brand, new BrandNotFoundException(command.BrandId));

		var supplier = await catalogDbContext.FindSupplierByIdAsync(SupplierId.Of(command.SupplierId));
		Guard.Against.NotFound(supplier, new SupplierNotFoundException(command.SupplierId));

		// await _domainEventDispatcher.DispatchAsync(cancellationToken, new Events.Domain.CreatingProduct());
		var product = Product.Create(
			ProductId.Of(command.Id),
			Name.Of(command.Name),
			Stock.Of(command.Stock, command.RestockThreshold, command.MaxStockThreshold),
			command.Status,
			Dimensions.Of(command.Width, command.Height, command.Depth),
			Size.Of(command.Size),
			command.Color,
			command.Description,
			Price.Of(command.Price),
			category!.Id,
			supplier!.Id,
			brand!.Id,
			images
		);

		await catalogDbContext.Products.AddAsync(product, cancellationToken: cancellationToken);

		await catalogDbContext.SaveChangesAsync(cancellationToken);

		var created = await catalogDbContext.Products
			.Include(x => x.Brand)
			.Include(x => x.Category)
			.Include(x => x.Supplier)
			.SingleOrDefaultAsync(x => x.Id == product.Id, cancellationToken: cancellationToken);

		var productDto = mapper.Map<ProductDto>(created);

		logger.LogInformation("Product a with ID: '{ProductId} created.'", command.Id);

		return new CreateProductResponse(productDto);
	}
}
