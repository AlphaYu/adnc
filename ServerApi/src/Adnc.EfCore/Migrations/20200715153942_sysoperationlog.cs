using Microsoft.EntityFrameworkCore.Migrations;

namespace  Adnc.Infr.EfCore.Migrations
{
    public partial class sysoperationlog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "SysOperationLog",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Account",
                table: "SysOperationLog",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RemoteIpAddress",
                table: "SysOperationLog",
                maxLength: 22,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "SysOperationLog",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Account",
                table: "SysOperationLog");

            migrationBuilder.DropColumn(
                name: "RemoteIpAddress",
                table: "SysOperationLog");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "SysOperationLog");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "SysOperationLog",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);
        }
    }
}
