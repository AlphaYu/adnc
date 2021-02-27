using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Adnc.Whse.Migrations.Migrations
{
    public partial class Init20210227 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    RowVersion = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<long>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    Sku = table.Column<string>(maxLength: 32, nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    Describe = table.Column<string>(maxLength: 128, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    StatusCode = table.Column<int>(nullable: true),
                    StatusChangesReason = table.Column<string>(maxLength: 32, nullable: true),
                    Unit = table.Column<string>(maxLength: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shelf",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    RowVersion = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<long>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    ProductId = table.Column<long>(nullable: true),
                    Qty = table.Column<int>(nullable: false),
                    FreezedQty = table.Column<int>(nullable: false),
                    PositionCode = table.Column<string>(maxLength: 32, nullable: true),
                    PositionDescription = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shelf", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Shelf");
        }
    }
}
