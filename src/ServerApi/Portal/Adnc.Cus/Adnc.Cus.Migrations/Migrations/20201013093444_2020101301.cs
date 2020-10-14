using Microsoft.EntityFrameworkCore.Migrations;

namespace Adnc.Cus.Migrations.Migrations
{
    public partial class _2020101301 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "CusFinance");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                table: "CusFinance",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
