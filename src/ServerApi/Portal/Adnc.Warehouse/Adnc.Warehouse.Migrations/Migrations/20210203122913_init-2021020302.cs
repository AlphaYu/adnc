using Microsoft.EntityFrameworkCore.Migrations;

namespace Adnc.Warehouse.Migrations.Migrations
{
    public partial class init2021020302 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status_StatusCode",
                table: "Product",
                newName: "StatusCode");

            migrationBuilder.RenameColumn(
                name: "Status_ChangeStatusReason",
                table: "Product",
                newName: "ChangeStatusReason");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StatusCode",
                table: "Product",
                newName: "Status_StatusCode");

            migrationBuilder.RenameColumn(
                name: "ChangeStatusReason",
                table: "Product",
                newName: "Status_ChangeStatusReason");
        }
    }
}
