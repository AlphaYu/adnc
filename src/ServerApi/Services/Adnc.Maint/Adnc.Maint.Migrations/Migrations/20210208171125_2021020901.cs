using Microsoft.EntityFrameworkCore.Migrations;

namespace Adnc.Maint.Migrations.Migrations
{
    public partial class _2021020901 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tips",
                table: "SysDict");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "SysDict",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "SysDict");

            migrationBuilder.AddColumn<string>(
                name: "Tips",
                table: "SysDict",
                type: "varchar(64) CHARACTER SET utf8mb4",
                maxLength: 64,
                nullable: true);
        }
    }
}
