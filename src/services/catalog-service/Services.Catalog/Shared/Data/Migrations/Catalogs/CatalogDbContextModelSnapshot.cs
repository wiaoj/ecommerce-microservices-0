﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Services.Catalog.Shared.Data;

#nullable disable

namespace ECommerce.Services.Catalogs.Shared.Data.Migrations.Catalogs {
	[DbContext(typeof(CatalogDbContext))]
    partial class CatalogDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ECommerce.Services.Catalogs.Brands.Brand", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created")
                        .HasDefaultValueSql("now()");

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("integer")
                        .HasColumnName("created_by");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("name");

                    b.Property<long>("OriginalVersion")
                        .IsConcurrencyToken()
                        .HasColumnType("bigint")
                        .HasColumnName("original_version");

                    b.HasKey("Id")
                        .HasName("pk_brands");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_brands_id");

                    b.ToTable("brands", "catalog");
                });

            modelBuilder.Entity("ECommerce.Services.Catalogs.Categories.Category", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("code");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created")
                        .HasDefaultValueSql("now()");

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("integer")
                        .HasColumnName("created_by");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("name");

                    b.Property<long>("OriginalVersion")
                        .IsConcurrencyToken()
                        .HasColumnType("bigint")
                        .HasColumnName("original_version");

                    b.HasKey("Id")
                        .HasName("pk_categories");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_categories_id");

                    b.ToTable("categories", "catalog");
                });

            modelBuilder.Entity("ECommerce.Services.Catalogs.Products.Models.Product", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<long>("BrandId")
                        .HasColumnType("bigint")
                        .HasColumnName("brand_id");

                    b.Property<long>("CategoryId")
                        .HasColumnType("bigint")
                        .HasColumnName("category_id");

                    b.Property<string>("Color")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(25)
                        .HasColumnType("character varying(25)")
                        .HasDefaultValue("Black")
                        .HasColumnName("color");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created")
                        .HasDefaultValueSql("now()");

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("integer")
                        .HasColumnName("created_by");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("name");

                    b.Property<long>("OriginalVersion")
                        .IsConcurrencyToken()
                        .HasColumnType("bigint")
                        .HasColumnName("original_version");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric(18,2)")
                        .HasColumnName("price");

                    b.Property<string>("ProductStatus")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(25)
                        .HasColumnType("character varying(25)")
                        .HasDefaultValue("Available")
                        .HasColumnName("product_status");

                    b.Property<string>("Size")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("size");

                    b.Property<long>("SupplierId")
                        .HasColumnType("bigint")
                        .HasColumnName("supplier_id");

                    b.HasKey("Id")
                        .HasName("pk_products");

                    b.HasIndex("BrandId")
                        .HasDatabaseName("ix_products_brand_id");

                    b.HasIndex("CategoryId")
                        .HasDatabaseName("ix_products_category_id");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_products_id");

                    b.HasIndex("SupplierId")
                        .HasDatabaseName("ix_products_supplier_id");

                    b.ToTable("products", "catalog");
                });

            modelBuilder.Entity("ECommerce.Services.Catalogs.Products.Models.ProductImage", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("integer")
                        .HasColumnName("created_by");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("image_url");

                    b.Property<bool>("IsMain")
                        .HasColumnType("boolean")
                        .HasColumnName("is_main");

                    b.Property<long>("ProductId")
                        .HasColumnType("bigint")
                        .HasColumnName("product_id");

                    b.HasKey("Id")
                        .HasName("pk_product_images");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_product_images_id");

                    b.HasIndex("ProductId")
                        .HasDatabaseName("ix_product_images_product_id");

                    b.ToTable("product_images", "catalog");
                });

            modelBuilder.Entity("ECommerce.Services.Catalogs.Products.Models.ProductView", b =>
                {
                    b.Property<long>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("product_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("ProductId"));

                    b.Property<long>("BrandId")
                        .HasColumnType("bigint")
                        .HasColumnName("brand_id");

                    b.Property<string>("BrandName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("brand_name");

                    b.Property<long>("CategoryId")
                        .HasColumnType("bigint")
                        .HasColumnName("category_id");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("category_name");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("product_name");

                    b.Property<long>("SupplierId")
                        .HasColumnType("bigint")
                        .HasColumnName("supplier_id");

                    b.Property<string>("SupplierName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("supplier_name");

                    b.HasKey("ProductId")
                        .HasName("pk_product_views");

                    b.HasIndex("ProductId")
                        .IsUnique()
                        .HasDatabaseName("ix_product_views_product_id");

                    b.ToTable("product_views", "catalog");
                });

            modelBuilder.Entity("ECommerce.Services.Catalogs.Suppliers.Supplier", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created")
                        .HasDefaultValueSql("now()");

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("integer")
                        .HasColumnName("created_by");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_suppliers");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("ix_suppliers_id");

                    b.ToTable("suppliers", "catalog");
                });

            modelBuilder.Entity("ECommerce.Services.Catalogs.Products.Models.Product", b =>
                {
                    b.HasOne("ECommerce.Services.Catalogs.Brands.Brand", "Brand")
                        .WithMany()
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_products_brands_brand_temp_id");

                    b.HasOne("ECommerce.Services.Catalogs.Categories.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_products_categories_category_temp_id");

                    b.HasOne("ECommerce.Services.Catalogs.Suppliers.Supplier", "Supplier")
                        .WithMany()
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_products_suppliers_supplier_temp_id");

                    b.OwnsOne("ECommerce.Services.Catalogs.Products.ValueObjects.Dimensions", "Dimensions", b1 =>
                        {
                            b1.Property<long>("ProductId")
                                .HasColumnType("bigint")
                                .HasColumnName("id");

                            b1.Property<int>("Depth")
                                .HasColumnType("integer")
                                .HasColumnName("dimensions_depth");

                            b1.Property<int>("Height")
                                .HasColumnType("integer")
                                .HasColumnName("dimensions_height");

                            b1.Property<int>("Width")
                                .HasColumnType("integer")
                                .HasColumnName("dimensions_width");

                            b1.HasKey("ProductId");

                            b1.ToTable("products", "catalog");

                            b1.WithOwner()
                                .HasForeignKey("ProductId")
                                .HasConstraintName("fk_products_products_id");
                        });

                    b.OwnsOne("ECommerce.Services.Catalogs.Products.ValueObjects.Stock", "Stock", b1 =>
                        {
                            b1.Property<long>("ProductId")
                                .HasColumnType("bigint")
                                .HasColumnName("id");

                            b1.Property<int>("Available")
                                .HasColumnType("integer")
                                .HasColumnName("stock_available");

                            b1.Property<int>("MaxStockThreshold")
                                .HasColumnType("integer")
                                .HasColumnName("stock_max_stock_threshold");

                            b1.Property<int>("RestockThreshold")
                                .HasColumnType("integer")
                                .HasColumnName("stock_restock_threshold");

                            b1.HasKey("ProductId");

                            b1.ToTable("products", "catalog");

                            b1.WithOwner()
                                .HasForeignKey("ProductId")
                                .HasConstraintName("fk_products_products_id");
                        });

                    b.Navigation("Brand");

                    b.Navigation("Category");

                    b.Navigation("Dimensions")
                        .IsRequired();

                    b.Navigation("Stock")
                        .IsRequired();

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("ECommerce.Services.Catalogs.Products.Models.ProductImage", b =>
                {
                    b.HasOne("ECommerce.Services.Catalogs.Products.Models.Product", "Product")
                        .WithMany("Images")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_product_images_products_product_id");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("ECommerce.Services.Catalogs.Products.Models.Product", b =>
                {
                    b.Navigation("Images");
                });
#pragma warning reecommerce 612, 618
        }
    }
}
