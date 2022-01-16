using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ptud_project.Migrations
{
    public partial class initdb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    id_area = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    area_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    area_description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.id_area);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    id_cus = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    created_at = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<long>(type: "bigint", nullable: false),
                    cmnd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sex = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.id_cus);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    id_payment = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    payment_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.id_payment);
                });

            migrationBuilder.CreateTable(
                name: "ShippingServices",
                columns: table => new
                {
                    id_ship = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    shipping_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    rating = table.Column<float>(type: "real", nullable: false),
                    shipping_fee = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingServices", x => x.id_ship);
                });

            migrationBuilder.CreateTable(
                name: "Providers",
                columns: table => new
                {
                    id_prov = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    rating = table.Column<short>(type: "smallint", nullable: false),
                    phone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    owner = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.id_prov);
                    table.ForeignKey(
                        name: "FK_Providers_Customers_owner",
                        column: x => x.owner,
                        principalTable: "Customers",
                        principalColumn: "id_cus",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    id_order = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<long>(type: "bigint", nullable: false),
                    update_at = table.Column<long>(type: "bigint", nullable: false),
                    pay_at = table.Column<long>(type: "bigint", nullable: false),
                    total_item = table.Column<short>(type: "smallint", nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    status = table.Column<short>(type: "smallint", nullable: false),
                    area = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    id_ship = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    payment_method = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    id_customer = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    id_provider = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.id_order);
                    table.ForeignKey(
                        name: "FK_Orders_Areas_area",
                        column: x => x.area,
                        principalTable: "Areas",
                        principalColumn: "id_area",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_id_customer",
                        column: x => x.id_customer,
                        principalTable: "Customers",
                        principalColumn: "id_cus",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Payments_payment_method",
                        column: x => x.payment_method,
                        principalTable: "Payments",
                        principalColumn: "id_payment",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Providers_id_provider",
                        column: x => x.id_provider,
                        principalTable: "Providers",
                        principalColumn: "id_prov",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_ShippingServices_id_ship",
                        column: x => x.id_ship,
                        principalTable: "ShippingServices",
                        principalColumn: "id_ship",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    id_product = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    product_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    rating = table.Column<float>(type: "real", nullable: false),
                    product_remaining = table.Column<long>(type: "bigint", nullable: false),
                    sell_number = table.Column<long>(type: "bigint", nullable: false),
                    create_at = table.Column<short>(type: "smallint", nullable: false),
                    update_at = table.Column<short>(type: "smallint", nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    supplier = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.id_product);
                    table.ForeignKey(
                        name: "FK_Products_Providers_supplier",
                        column: x => x.supplier,
                        principalTable: "Providers",
                        principalColumn: "id_prov",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DetailOrders",
                columns: table => new
                {
                    order_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    unit_price = table.Column<long>(type: "bigint", nullable: false),
                    quantity = table.Column<short>(type: "smallint", nullable: false),
                    total = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailOrders", x => new { x.order_id, x.product_id });
                    table.ForeignKey(
                        name: "FK_DetailOrders_Orders_order_id",
                        column: x => x.order_id,
                        principalTable: "Orders",
                        principalColumn: "id_order",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetailOrders_Products_product_id",
                        column: x => x.product_id,
                        principalTable: "Products",
                        principalColumn: "id_product",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_phone",
                table: "Customers",
                column: "phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DetailOrders_product_id",
                table: "DetailOrders",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_area",
                table: "Orders",
                column: "area");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_id_customer",
                table: "Orders",
                column: "id_customer");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_id_provider",
                table: "Orders",
                column: "id_provider");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_id_ship",
                table: "Orders",
                column: "id_ship");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_payment_method",
                table: "Orders",
                column: "payment_method");

            migrationBuilder.CreateIndex(
                name: "IX_Products_supplier",
                table: "Products",
                column: "supplier");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_owner",
                table: "Providers",
                column: "owner");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetailOrders");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "ShippingServices");

            migrationBuilder.DropTable(
                name: "Providers");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
