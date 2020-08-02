using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace  Adnc.Infr.EfCore.Migrations
{
    public partial class userrowversion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RowVersion",
                table: "SysUser",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "SysUser");

        }
    }
}
