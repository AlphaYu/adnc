using Microsoft.EntityFrameworkCore.Migrations;

namespace  Adnc.Infr.EfCore.Migrations
{
    public partial class menu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SysRelation_MenuId",
                table: "SysRelation",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_SysRelation_SysMenu_MenuId",
                table: "SysRelation",
                column: "MenuId",
                principalTable: "SysMenu",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SysRelation_SysMenu_MenuId",
                table: "SysRelation");

            migrationBuilder.DropIndex(
                name: "IX_SysRelation_MenuId",
                table: "SysRelation");
        }
    }
}
