using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Adnc.Warehouse.Migrations.Migrations
{
    public partial class init20210203 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Sku = table.Column<string>(maxLength: 32, nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    Describe = table.Column<string>(maxLength: 128, nullable: true),
                    Price = table.Column<float>(nullable: false),
                    Status_StatusCode = table.Column<int>(nullable: true),
                    Status_ChangeStatusReason = table.Column<string>(maxLength: 32, nullable: true),
                    ShlefId = table.Column<long>(nullable: true),
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
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
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
