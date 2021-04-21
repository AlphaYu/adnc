using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Adnc.Cus.Migrations.Migrations
{
    public partial class Init2021030301 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    CreateBy = table.Column<long>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<long>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Account = table.Column<string>(maxLength: 16, nullable: false),
                    Nickname = table.Column<string>(maxLength: 16, nullable: false),
                    Realname = table.Column<string>(maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerFinance",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    CreateBy = table.Column<long>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<long>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Account = table.Column<string>(maxLength: 16, nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    RowVersion = table.Column<DateTime>(rowVersion: true, nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerFinance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerFinance_Customer_Id",
                        column: x => x.Id,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerTransactionLog",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    CreateBy = table.Column<long>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    CustomerId = table.Column<long>(nullable: false),
                    Account = table.Column<string>(maxLength: 16, nullable: false),
                    ExchangeType = table.Column<int>(nullable: false),
                    ExchageStatus = table.Column<int>(nullable: false),
                    ChangingAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ChangedAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Remark = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTransactionLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerTransactionLog_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTransactionLog_CustomerId",
                table: "CustomerTransactionLog",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerFinance");

            migrationBuilder.DropTable(
                name: "CustomerTransactionLog");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
