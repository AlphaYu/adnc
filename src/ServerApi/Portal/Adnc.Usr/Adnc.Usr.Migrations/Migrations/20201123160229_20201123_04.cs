using Microsoft.EntityFrameworkCore.Migrations;

namespace Adnc.Usr.Migrations.Migrations
{
    public partial class _20201123_04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Component",
                table: "SysMenu",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(48) CHARACTER SET utf8mb4",
                oldMaxLength: 48,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Component",
                table: "SysMenu",
                type: "varchar(48) CHARACTER SET utf8mb4",
                maxLength: 48,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 64,
                oldNullable: true);
        }
    }
}
