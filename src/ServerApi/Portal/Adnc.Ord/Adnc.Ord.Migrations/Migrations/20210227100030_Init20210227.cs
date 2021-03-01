using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Adnc.Ord.Migrations.Migrations
{
    public partial class Init20210227 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    RowVersion = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<long>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    CustomerId = table.Column<long>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Remark = table.Column<string>(maxLength: 64, nullable: true),
                    StatusCode = table.Column<int>(nullable: true),
                    StatusChangesReason = table.Column<string>(maxLength: 32, nullable: true),
                    ReceiverName = table.Column<string>(maxLength: 16, nullable: true),
                    ReceiverPhone = table.Column<string>(maxLength: 11, nullable: true),
                    ReceiverAddress = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    OrderId = table.Column<long>(nullable: false),
                    ProductId = table.Column<long>(nullable: true),
                    ProductName = table.Column<string>(maxLength: 64, nullable: true),
                    ProductPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    Count = table.Column<int>(nullable: false)
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
                });

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
