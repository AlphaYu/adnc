using Microsoft.EntityFrameworkCore.Migrations;

namespace  Adnc.Infr.EfCore.Migrations
{
    public partial class cfg7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SysDict",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SysCfg",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SysDict");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SysCfg");
        }
    }
}
