using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ptud_project.Migrations
{
    public partial class FixDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

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
                    sex = table.Column<short>(type: "smallint", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.id_cus);
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

            migrationBuilder.CreateIndex(
                name: "IX_Customers_phone",
                table: "Customers",
                column: "phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Providers_owner",
                table: "Providers",
                column: "owner");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Providers");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    user_name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    avartar_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<long>(type: "bigint", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.user_name);
                });
        }
    }
}
