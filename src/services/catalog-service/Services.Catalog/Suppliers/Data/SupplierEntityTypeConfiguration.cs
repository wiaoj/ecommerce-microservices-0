using BuildingBlocks.Core.Persistence.EfCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Catalog.Shared.Data;

namespace Services.Catalog.Suppliers.Data;

public class SupplierEntityTypeConfiguration : IEntityTypeConfiguration<Supplier> {
	public void Configure(EntityTypeBuilder<Supplier> builder) {
		builder.ToTable("suppliers", CatalogDbContext.DefaultSchema);
		builder.HasKey(x => x.Id);
		builder.HasIndex(x => x.Id).IsUnique();
		builder.Property(x => x.Id)
			.HasConversion(x => x.Value, x => x)
			.ValueGeneratedNever();

		builder.Property(x => x.Created).HasDefaultValueSql(EfConstants.DateAlgorithm);
		builder.Property(x => x.Name).HasColumnType(EfConstants.ColumnTypes.NormalText).IsRequired();
	}
}
