using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adnc.Demo.Ord.Migrations.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4 ");

            migrationBuilder.CreateTable(
                name: "eventtracker",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                    eventid = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                    trackername = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    createby = table.Column<long>(type: "bigint", nullable: false, comment: "创建人"),
                    createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间/注册时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_eventtracker", x => x.id);
                },
                comment: "事件跟踪/处理信息")
                .Annotation("MySql:CharSet", "utf8mb4 ");

            migrationBuilder.CreateTable(
                name: "order",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                    customerid = table.Column<long>(type: "bigint", nullable: false, comment: "客户Id"),
                    amount = table.Column<decimal>(type: "decimal(18,4)", nullable: false, comment: "订单金额"),
                    remark = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true, comment: "备注")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    statuscode = table.Column<int>(type: "int", nullable: false),
                    statuschangesreason = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    receivername = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    receiverphone = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    receiveraddress = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    rowversion = table.Column<DateTime>(type: "timestamp(6)", rowVersion: true, nullable: false, comment: "")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    createby = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                    createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order", x => x.id);
                },
                comment: "订单")
                .Annotation("MySql:CharSet", "utf8mb4 ");

            migrationBuilder.CreateTable(
                name: "orderitem",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                    orderid = table.Column<long>(type: "bigint", nullable: false, comment: "订单Id"),
                    producid = table.Column<long>(type: "bigint", nullable: false),
                    productname = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    productprice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    count = table.Column<int>(type: "int", nullable: false, comment: "数量")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orderitem", x => x.id);
                    table.ForeignKey(
                        name: "fk_orderitem_order_orderid",
                        column: x => x.orderid,
                        principalTable: "order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "订单条目")
                .Annotation("MySql:CharSet", "utf8mb4 ");

            migrationBuilder.CreateIndex(
                name: "ix_eventtracker_eventid_trackername",
                table: "eventtracker",
                columns: new[] { "eventid", "trackername" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_orderitem_orderid",
                table: "orderitem",
                column: "orderid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "eventtracker");

            migrationBuilder.DropTable(
                name: "orderitem");

            migrationBuilder.DropTable(
                name: "order");
        }
    }
}
