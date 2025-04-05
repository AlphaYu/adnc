using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adnc.Demo.Cust.Api.Migrations;

/// <inheritdoc />
public partial class Init20250317 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.CreateTable(
            name: "cust_customer",
            columns: table => new
            {
                id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                createby = table.Column<long>(type: "bigint", nullable: false, comment: "创建人"),
                createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间/注册时间"),
                modifyby = table.Column<long>(type: "bigint", nullable: false, comment: "最后更新人"),
                modifytime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "最后更新时间"),
                account = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, comment: "")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                password = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                nickname = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, comment: "")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                realname = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, comment: "")
                    .Annotation("MySql:CharSet", "utf8mb4 ")
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_cust_customer", x => x.id);
            },
            comment: "客户表")
            .Annotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.CreateTable(
            name: "cust_eventtracker",
            columns: table => new
            {
                id = table.Column<long>(type: "bigint", nullable: false, comment: "")
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                eventid = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                trackername = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                createby = table.Column<long>(type: "bigint", nullable: false, comment: "创建人"),
                createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间/注册时间")
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_cust_eventtracker", x => x.id);
            },
            comment: "事件跟踪/处理信息")
            .Annotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.CreateTable(
            name: "cust_finance",
            columns: table => new
            {
                id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                rowversion = table.Column<DateTime>(type: "timestamp(6)", rowVersion: true, nullable: false, comment: ""),
                createby = table.Column<long>(type: "bigint", nullable: false, comment: "创建人"),
                createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间/注册时间"),
                modifyby = table.Column<long>(type: "bigint", nullable: false, comment: "最后更新人"),
                modifytime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "最后更新时间"),
                account = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, comment: "")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                balance = table.Column<decimal>(type: "decimal(18,4)", nullable: false, comment: "")
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_cust_finance", x => x.id);
                table.ForeignKey(
                    name: "fk_cust_finance_cust_customer_id",
                    column: x => x.id,
                    principalTable: "cust_customer",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            },
            comment: "客户财务表")
            .Annotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.CreateTable(
            name: "cust_transactionlog",
            columns: table => new
            {
                id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                createby = table.Column<long>(type: "bigint", nullable: false, comment: "创建人"),
                createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间/注册时间"),
                customerid = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                account = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, comment: "")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                exchangetype = table.Column<int>(type: "int", nullable: false, comment: ""),
                exchagestatus = table.Column<int>(type: "int", nullable: false, comment: ""),
                changingamount = table.Column<decimal>(type: "decimal(18,4)", nullable: false, comment: ""),
                amount = table.Column<decimal>(type: "decimal(18,4)", nullable: false, comment: ""),
                changedamount = table.Column<decimal>(type: "decimal(18,4)", nullable: false, comment: ""),
                remark = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false, comment: "")
                    .Annotation("MySql:CharSet", "utf8mb4 ")
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_cust_transactionlog", x => x.id);
                table.ForeignKey(
                    name: "fk_cust_transactionlog_cust_customer_customerid",
                    column: x => x.customerid,
                    principalTable: "cust_customer",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            },
            comment: "客户财务变动记录")
            .Annotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.CreateIndex(
            name: "ix_cust_eventtracker_eventid_trackername",
            table: "cust_eventtracker",
            columns: ["eventid", "trackername"],
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_cust_transactionlog_customerid",
            table: "cust_transactionlog",
            column: "customerid");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "cust_eventtracker");

        migrationBuilder.DropTable(
            name: "cust_finance");

        migrationBuilder.DropTable(
            name: "cust_transactionlog");

        migrationBuilder.DropTable(
            name: "cust_customer");
    }
}
