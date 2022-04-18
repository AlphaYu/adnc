using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Adnc.Ord.Migrations.Migrations
{
    public partial class init20210701 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4 ");

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Remark = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    StatusCode = table.Column<int>(type: "int", nullable: true),
                    StatusChangesReason = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    ReceiverName = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    ReceiverPhone = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    ReceiverAddress = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    RowVersion = table.Column<DateTime>(type: "timestamp(6)", rowVersion: true, nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4 ");

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    ProductName = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    ProductPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItem_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4 ");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderId",
                table: "OrderItem",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItem");

            migrationBuilder.DropTable(
                name: "Order");
        }
    }
}
