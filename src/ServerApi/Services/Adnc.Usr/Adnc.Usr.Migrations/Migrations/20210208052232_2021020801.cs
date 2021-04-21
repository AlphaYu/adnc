using Microsoft.EntityFrameworkCore.Migrations;

namespace Adnc.Usr.Migrations.Migrations
{
    public partial class _2021020801 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "SysUser");

            migrationBuilder.DropColumn(
                name: "Num",
                table: "SysRole");

            migrationBuilder.DropColumn(
                name: "Num",
                table: "SysMenu");

            migrationBuilder.DropColumn(
                name: "Num",
                table: "SysDept");

            migrationBuilder.AddColumn<string>(
                name: "RoleIds",
                table: "SysUser",
                maxLength: 72,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Ordinal",
                table: "SysRole",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Ordinal",
                table: "SysMenu",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Ordinal",
                table: "SysDept",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleIds",
                table: "SysUser");

            migrationBuilder.DropColumn(
                name: "Ordinal",
                table: "SysRole");

            migrationBuilder.DropColumn(
                name: "Ordinal",
                table: "SysMenu");

            migrationBuilder.DropColumn(
                name: "Ordinal",
                table: "SysDept");

            migrationBuilder.AddColumn<string>(
                name: "RoleId",
                table: "SysUser",
                type: "varchar(72) CHARACTER SET utf8mb4",
                maxLength: 72,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Num",
                table: "SysRole",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Num",
                table: "SysMenu",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Num",
                table: "SysDept",
                type: "int",
                nullable: true);
        }
    }
}
