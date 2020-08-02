using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace  Adnc.Infr.EfCore.Migrations
{
    public partial class userfinacerowversion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "SysUserFinance",
                type: "timestamp(3)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(3)",
                oldDefaultValueSql: "'0000-00-00 00:00:00.000'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "SysUserFinance",
                type: "timestamp(3)",
                nullable: false,
                defaultValueSql: "'0000-00-00 00:00:00.000'",
                oldClrType: typeof(DateTime),
                oldType: "timestamp(3)",
                oldNullable: true);
        }
    }
}
