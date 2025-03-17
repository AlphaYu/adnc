using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adnc.Demo.Cust.Api.Migrations
{
    /// <inheritdoc />
    public partial class Update20250317 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customerfinance");

            migrationBuilder.DropTable(
                name: "customertransactionlog");

            migrationBuilder.DropPrimaryKey(
                name: "pk_eventtracker",
                table: "eventtracker");

            migrationBuilder.DropPrimaryKey(
                name: "pk_customer",
                table: "customer");

            migrationBuilder.RenameTable(
                name: "eventtracker",
                newName: "cust_eventtracker");

            migrationBuilder.RenameTable(
                name: "customer",
                newName: "cust_customer");

            migrationBuilder.RenameIndex(
                name: "ix_eventtracker_eventid_trackername",
                table: "cust_eventtracker",
                newName: "ix_cust_eventtracker_eventid_trackername");

            migrationBuilder.AlterColumn<DateTime>(
                name: "modifytime",
                table: "cust_customer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "最后更新时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true,
                oldComment: "最后更新时间")
                .Annotation("Relational:ColumnOrder", 103);

            migrationBuilder.AlterColumn<long>(
                name: "modifyby",
                table: "cust_customer",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "最后更新人",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "最后更新人")
                .Annotation("Relational:ColumnOrder", 102);

            migrationBuilder.AlterColumn<DateTime>(
                name: "createtime",
                table: "cust_customer",
                type: "datetime(6)",
                nullable: false,
                comment: "创建时间/注册时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "创建时间/注册时间")
                .Annotation("Relational:ColumnOrder", 101);

            migrationBuilder.AlterColumn<long>(
                name: "createby",
                table: "cust_customer",
                type: "bigint",
                nullable: false,
                comment: "创建人",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "创建人")
                .Annotation("Relational:ColumnOrder", 100);

            migrationBuilder.AddPrimaryKey(
                name: "pk_cust_eventtracker",
                table: "cust_eventtracker",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_cust_customer",
                table: "cust_customer",
                column: "id");

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
                name: "ix_cust_transactionlog_customerid",
                table: "cust_transactionlog",
                column: "customerid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cust_finance");

            migrationBuilder.DropTable(
                name: "cust_transactionlog");

            migrationBuilder.DropPrimaryKey(
                name: "pk_cust_eventtracker",
                table: "cust_eventtracker");

            migrationBuilder.DropPrimaryKey(
                name: "pk_cust_customer",
                table: "cust_customer");

            migrationBuilder.RenameTable(
                name: "cust_eventtracker",
                newName: "eventtracker");

            migrationBuilder.RenameTable(
                name: "cust_customer",
                newName: "customer");

            migrationBuilder.RenameIndex(
                name: "ix_cust_eventtracker_eventid_trackername",
                table: "eventtracker",
                newName: "ix_eventtracker_eventid_trackername");

            migrationBuilder.AlterColumn<DateTime>(
                name: "modifytime",
                table: "customer",
                type: "datetime(6)",
                nullable: true,
                comment: "最后更新时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "最后更新时间")
                .OldAnnotation("Relational:ColumnOrder", 103);

            migrationBuilder.AlterColumn<long>(
                name: "modifyby",
                table: "customer",
                type: "bigint",
                nullable: true,
                comment: "最后更新人",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "最后更新人")
                .OldAnnotation("Relational:ColumnOrder", 102);

            migrationBuilder.AlterColumn<DateTime>(
                name: "createtime",
                table: "customer",
                type: "datetime(6)",
                nullable: false,
                comment: "创建时间/注册时间",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "创建时间/注册时间")
                .OldAnnotation("Relational:ColumnOrder", 101);

            migrationBuilder.AlterColumn<long>(
                name: "createby",
                table: "customer",
                type: "bigint",
                nullable: false,
                comment: "创建人",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "创建人")
                .OldAnnotation("Relational:ColumnOrder", 100);

            migrationBuilder.AddPrimaryKey(
                name: "pk_eventtracker",
                table: "eventtracker",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_customer",
                table: "customer",
                column: "id");

            migrationBuilder.CreateTable(
                name: "customerfinance",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                    account = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, comment: "")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    balance = table.Column<decimal>(type: "decimal(18,4)", nullable: false, comment: ""),
                    createby = table.Column<long>(type: "bigint", nullable: false, comment: "创建人"),
                    createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间/注册时间"),
                    modifyby = table.Column<long>(type: "bigint", nullable: true, comment: "最后更新人"),
                    modifytime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "最后更新时间"),
                    rowversion = table.Column<DateTime>(type: "timestamp(6)", rowVersion: true, nullable: false, comment: "")
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
                    account = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, comment: "")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    amount = table.Column<decimal>(type: "decimal(18,4)", nullable: false, comment: ""),
                    changedamount = table.Column<decimal>(type: "decimal(18,4)", nullable: false, comment: ""),
                    changingamount = table.Column<decimal>(type: "decimal(18,4)", nullable: false, comment: ""),
                    createby = table.Column<long>(type: "bigint", nullable: false, comment: "创建人"),
                    createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间/注册时间"),
                    customerid = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                    exchagestatus = table.Column<int>(type: "int", nullable: false, comment: ""),
                    exchangetype = table.Column<int>(type: "int", nullable: false, comment: ""),
                    remark = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false, comment: "")
                        .Annotation("MySql:CharSet", "utf8mb4 ")
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
        }
    }
}
