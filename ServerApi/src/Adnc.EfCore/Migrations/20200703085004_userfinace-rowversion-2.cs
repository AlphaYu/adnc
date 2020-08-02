using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace  Adnc.Infr.EfCore.Migrations
{
    public partial class userfinacerowversion2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "SysUserFinance",
                type: "timestamp(3)",
                nullable: true,
                defaultValueSql: "'2000-07-01 22:33:02.559'",
                oldClrType: typeof(DateTime),
                oldType: "timestamp(3)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "SysUserFinance",
                type: "timestamp(3)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(3)",
                oldNullable: true,
                oldDefaultValueSql: "'2000-07-01 22:33:02.559'");
        }
    }
}
