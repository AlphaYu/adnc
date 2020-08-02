using Microsoft.EntityFrameworkCore.Migrations;

namespace  Adnc.Infr.EfCore.Migrations
{
    public partial class userfinacerole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SysRelation_RoleId",
                table: "SysRelation",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_SysRelation_SysRole_RoleId",
                table: "SysRelation",
                column: "RoleId",
                principalTable: "SysRole",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SysRelation_SysRole_RoleId",
                table: "SysRelation");

            migrationBuilder.DropIndex(
                name: "IX_SysRelation_RoleId",
                table: "SysRelation");
        }
    }
}
