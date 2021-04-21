using Microsoft.EntityFrameworkCore.Migrations;

namespace Adnc.Usr.Migrations.Migrations
{
    public partial class _20201124_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "SysUser",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(64) CHARACTER SET utf8mb4",
                oldMaxLength: 64,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "SysUser",
                type: "varchar(64) CHARACTER SET utf8mb4",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 32,
                oldNullable: true);
        }
    }
}
