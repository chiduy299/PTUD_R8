using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ptud_project.Migrations
{
    public partial class Define_Model_Data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    id_area = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    area_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    area_description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.id_area);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    id_payment = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    payment_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.id_payment);
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
                name: "ShippingServices",
                columns: table => new
                {
                    id_ship = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    shipping_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    rating = table.Column<float>(type: "real", nullable: false),
                    shipping_fee = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingServices", x => x.id_ship);
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
                    area = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    shipping_method = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    payment_method = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                        name: "FK_Orders_Payments_payment_method",
                        column: x => x.payment_method,
                        principalTable: "Payments",
                        principalColumn: "id_payment",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_ShippingServices_shipping_method",
                        column: x => x.shipping_method,
                        principalTable: "ShippingServices",
                        principalColumn: "id_ship",
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
                name: "IX_DetailOrders_product_id",
                table: "DetailOrders",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_area",
                table: "Orders",
                column: "area");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_payment_method",
                table: "Orders",
                column: "payment_method");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_shipping_method",
                table: "Orders",
                column: "shipping_method");

            migrationBuilder.CreateIndex(
                name: "IX_Products_supplier",
                table: "Products",
                column: "supplier");
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
        }
    }
}
