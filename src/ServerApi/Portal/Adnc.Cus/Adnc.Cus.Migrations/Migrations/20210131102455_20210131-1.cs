using Microsoft.EntityFrameworkCore.Migrations;

namespace Adnc.Cus.Migrations.Migrations
{
    public partial class _202101311 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CusFinance_Customer_ID",
                table: "CusFinance");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "CusTransactionLog",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Customer",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "CusFinance",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CusFinance_Customer_Id",
                table: "CusFinance",
                column: "Id",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CusFinance_Customer_Id",
                table: "CusFinance");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CusTransactionLog",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Customer",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CusFinance",
                newName: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CusFinance_Customer_ID",
                table: "CusFinance",
                column: "ID",
                principalTable: "Customer",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
