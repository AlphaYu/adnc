using Microsoft.EntityFrameworkCore.Migrations;

namespace  Adnc.Infr.EfCore.Migrations
{
    public partial class user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_SysUser_SysDept_DeptId",
                table: "SysUser",
                column: "DeptId",
                principalTable: "SysDept",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
