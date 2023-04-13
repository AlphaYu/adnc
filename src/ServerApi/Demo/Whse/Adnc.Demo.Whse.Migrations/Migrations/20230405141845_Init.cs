using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adnc.Demo.Whse.Migrations.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "eventtracker",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                    eventid = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                    trackername = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: ""),
                    createby = table.Column<long>(type: "bigint", nullable: false, comment: "创建人"),
                    createtime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "创建时间/注册时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_eventtracker", x => x.id);
                },
                comment: "事件跟踪/处理信息");

            migrationBuilder.CreateTable(
                name: "inventorychangeslog",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "")
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_inventorychangeslog", x => x.id);
                },
                comment: "");

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                    sku = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false, comment: ""),
                    name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false, comment: ""),
                    describe = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false, comment: ""),
                    price = table.Column<decimal>(type: "decimal(18,4)", nullable: false, comment: ""),
                    statuscode = table.Column<int>(type: "int", nullable: false),
                    statuschangesreason = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    unit = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false, comment: ""),
                    rowversion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false, comment: ""),
                    createby = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                    createtime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product", x => x.id);
                },
                comment: "");

            migrationBuilder.CreateTable(
                name: "warehouse",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                    productid = table.Column<long>(type: "bigint", nullable: true, comment: ""),
                    qty = table.Column<int>(type: "int", nullable: false, comment: ""),
                    blockedqty = table.Column<int>(type: "int", nullable: false, comment: ""),
                    positioncode = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    positiondescription = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    rowversion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false, comment: ""),
                    createby = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                    createtime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_warehouse", x => x.id);
                },
                comment: "货架");

            migrationBuilder.CreateIndex(
                name: "ix_eventtracker_eventid_trackername",
                table: "eventtracker",
                columns: new[] { "eventid", "trackername" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "eventtracker");

            migrationBuilder.DropTable(
                name: "inventorychangeslog");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "warehouse");
        }
    }
}
