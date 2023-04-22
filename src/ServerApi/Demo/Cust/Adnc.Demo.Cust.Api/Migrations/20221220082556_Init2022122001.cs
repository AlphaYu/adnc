using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adnc.Demo.Cust.Api.Migrations
{
    public partial class Init2022122001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4 ");

            migrationBuilder.CreateTable(
                name: "customer",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                    account = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, comment: "")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    password = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    nickname = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, comment: "")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    realname = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, comment: "")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    createby = table.Column<long>(type: "bigint", nullable: false, comment: "创建人"),
                    createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间/注册时间"),
                    modifyby = table.Column<long>(type: "bigint", nullable: true, comment: "最后更新人"),
                    modifytime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "最后更新时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customer", x => x.id);
                },
                comment: "客户表")
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
                name: "customerfinance",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                    account = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, comment: "")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    balance = table.Column<decimal>(type: "decimal(18,4)", nullable: false, comment: ""),
                    rowversion = table.Column<DateTime>(type: "timestamp(6)", rowVersion: true, nullable: false, comment: "")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    createby = table.Column<long>(type: "bigint", nullable: false, comment: "创建人"),
                    createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间/注册时间"),
                    modifyby = table.Column<long>(type: "bigint", nullable: true, comment: "最后更新人"),
                    modifytime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "最后更新时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customerfinance", x => x.id);
                    table.ForeignKey(
                        name: "fk_customerfinance_customer_id",
                        column: x => x.id,
                        principalTable: "customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "客户财务表")
                .Annotation("MySql:CharSet", "utf8mb4 ");

            migrationBuilder.CreateTable(
                name: "customertransactionlog",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                    customerid = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                    account = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, comment: "")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    exchangetype = table.Column<int>(type: "int", nullable: false, comment: ""),
                    exchagestatus = table.Column<int>(type: "int", nullable: false, comment: ""),
                    changingamount = table.Column<decimal>(type: "decimal(18,4)", nullable: false, comment: ""),
                    amount = table.Column<decimal>(type: "decimal(18,4)", nullable: false, comment: ""),
                    changedamount = table.Column<decimal>(type: "decimal(18,4)", nullable: false, comment: ""),
                    remark = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false, comment: "")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    createby = table.Column<long>(type: "bigint", nullable: false, comment: "创建人"),
                    createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间/注册时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customertransactionlog", x => x.id);
                    table.ForeignKey(
                        name: "fk_customertransactionlog_customer_customerid",
                        column: x => x.customerid,
                        principalTable: "customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "客户财务变动记录")
                .Annotation("MySql:CharSet", "utf8mb4 ");

            migrationBuilder.CreateIndex(
                name: "ix_customertransactionlog_customerid",
                table: "customertransactionlog",
                column: "customerid");

            migrationBuilder.CreateIndex(
                name: "ix_eventtracker_eventid_trackername",
                table: "eventtracker",
                columns: new[] { "eventid", "trackername" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customerfinance");

            migrationBuilder.DropTable(
                name: "customertransactionlog");

            migrationBuilder.DropTable(
                name: "eventtracker");

            migrationBuilder.DropTable(
                name: "customer");
        }
    }
}
