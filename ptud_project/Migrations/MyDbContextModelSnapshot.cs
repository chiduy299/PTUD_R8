﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ptud_project.Data;

namespace ptud_project.Migrations
{
    [DbContext(typeof(MyDbContext))]
    partial class MyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ptud_project.Data.Area", b =>
                {
                    b.Property<string>("id_area")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("area_description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("area_name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("id_area");

                    b.ToTable("Areas");
                });

            modelBuilder.Entity("ptud_project.Data.Customer", b =>
                {
                    b.Property<Guid>("id_cus")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("avatar_url")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("cmnd")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("created_at")
                        .HasColumnType("bigint");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("phone")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<short>("sex")
                        .HasColumnType("smallint");

                    b.Property<long>("update_at")
                        .HasColumnType("bigint");

                    b.HasKey("id_cus");

                    b.HasIndex("phone")
                        .IsUnique();

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("ptud_project.Data.DetailOrder", b =>
                {
                    b.Property<Guid?>("order_id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("product_id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<short>("quantity")
                        .HasColumnType("smallint");

                    b.Property<long>("total")
                        .HasColumnType("bigint");

                    b.Property<long>("unit_price")
                        .HasColumnType("bigint");

                    b.HasKey("order_id", "product_id");

                    b.HasIndex("product_id");

                    b.ToTable("DetailOrders");
                });

            modelBuilder.Entity("ptud_project.Data.Order", b =>
                {
                    b.Property<Guid>("id_order")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("area")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("created_at")
                        .HasColumnType("bigint");

                    b.Property<Guid?>("id_customer")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("id_provider")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("id_ship")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("pay_at")
                        .HasColumnType("bigint");

                    b.Property<string>("payment_method")
                        .HasColumnType("nvarchar(450)");

                    b.Property<short>("status")
                        .HasColumnType("smallint");

                    b.Property<decimal>("total_amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<short>("total_item")
                        .HasColumnType("smallint");

                    b.Property<long>("update_at")
                        .HasColumnType("bigint");

                    b.HasKey("id_order");

                    b.HasIndex("area");

                    b.HasIndex("id_customer");

                    b.HasIndex("id_provider");

                    b.HasIndex("id_ship");

                    b.HasIndex("payment_method");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ptud_project.Data.Payment", b =>
                {
                    b.Property<string>("id_payment")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("payment_name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("id_payment");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("ptud_project.Data.Product", b =>
                {
                    b.Property<Guid>("id_product")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<short>("create_at")
                        .HasColumnType("smallint");

                    b.Property<string>("product_name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<long>("product_remaining")
                        .HasColumnType("bigint");

                    b.Property<float>("rating")
                        .HasColumnType("real");

                    b.Property<long>("sell_number")
                        .HasColumnType("bigint");

                    b.Property<Guid?>("supplier")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("total_amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<short>("update_at")
                        .HasColumnType("smallint");

                    b.HasKey("id_product");

                    b.HasIndex("supplier");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ptud_project.Data.Provider", b =>
                {
                    b.Property<Guid>("id_prov")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid?>("owner")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("phone")
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<short>("rating")
                        .HasColumnType("smallint");

                    b.HasKey("id_prov");

                    b.HasIndex("owner");

                    b.ToTable("Providers");
                });

            modelBuilder.Entity("ptud_project.Data.ShippingServices", b =>
                {
                    b.Property<string>("id_ship")
                        .HasColumnType("nvarchar(450)");

                    b.Property<float>("rating")
                        .HasColumnType("real");

                    b.Property<long>("shipping_fee")
                        .HasColumnType("bigint");

                    b.Property<string>("shipping_name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("id_ship");

                    b.ToTable("ShippingServices");
                });

            modelBuilder.Entity("ptud_project.Data.DetailOrder", b =>
                {
                    b.HasOne("ptud_project.Data.Order", "order")
                        .WithMany()
                        .HasForeignKey("order_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ptud_project.Data.Product", "product")
                        .WithMany()
                        .HasForeignKey("product_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("order");

                    b.Navigation("product");
                });

            modelBuilder.Entity("ptud_project.Data.Order", b =>
                {
                    b.HasOne("ptud_project.Data.Area", "area_order")
                        .WithMany()
                        .HasForeignKey("area");

                    b.HasOne("ptud_project.Data.Customer", "customer")
                        .WithMany()
                        .HasForeignKey("id_customer");

                    b.HasOne("ptud_project.Data.Provider", "provider")
                        .WithMany()
                        .HasForeignKey("id_provider");

                    b.HasOne("ptud_project.Data.ShippingServices", "Shipping")
                        .WithMany()
                        .HasForeignKey("id_ship");

                    b.HasOne("ptud_project.Data.Payment", "payment")
                        .WithMany()
                        .HasForeignKey("payment_method");

                    b.Navigation("area_order");

                    b.Navigation("customer");

                    b.Navigation("payment");

                    b.Navigation("provider");

                    b.Navigation("Shipping");
                });

            modelBuilder.Entity("ptud_project.Data.Product", b =>
                {
                    b.HasOne("ptud_project.Data.Provider", "provider")
                        .WithMany()
                        .HasForeignKey("supplier");

                    b.Navigation("provider");
                });

            modelBuilder.Entity("ptud_project.Data.Provider", b =>
                {
                    b.HasOne("ptud_project.Data.Customer", "customer")
                        .WithMany()
                        .HasForeignKey("owner");

                    b.Navigation("customer");
                });
#pragma warning restore 612, 618
        }
    }
}
