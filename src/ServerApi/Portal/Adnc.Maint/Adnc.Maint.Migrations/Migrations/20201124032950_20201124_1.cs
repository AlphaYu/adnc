using Microsoft.EntityFrameworkCore.Migrations;

namespace Adnc.Maint.Migrations.Migrations
{
    public partial class _20201124_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "SysLoginLog",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(64) CHARACTER SET utf8mb4",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RemoteIpAddress",
                table: "SysLoginLog",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(22) CHARACTER SET utf8mb4",
                oldMaxLength: 22,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Device",
                table: "SysLoginLog",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20) CHARACTER SET utf8mb4",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Account",
                table: "SysLoginLog",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(32) CHARACTER SET utf8mb4",
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CfgValue",
                table: "SysCfg",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(256) CHARACTER SET utf8mb4",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CfgDesc",
                table: "SysCfg",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(512) CHARACTER SET utf8mb4",
                oldMaxLength: 512,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "SysLoginLog",
                type: "varchar(64) CHARACTER SET utf8mb4",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 16,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RemoteIpAddress",
                table: "SysLoginLog",
                type: "varchar(22) CHARACTER SET utf8mb4",
                maxLength: 22,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Device",
                table: "SysLoginLog",
                type: "varchar(20) CHARACTER SET utf8mb4",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 16,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Account",
                table: "SysLoginLog",
                type: "varchar(32) CHARACTER SET utf8mb4",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 16,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CfgValue",
                table: "SysCfg",
                type: "varchar(256) CHARACTER SET utf8mb4",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CfgDesc",
                table: "SysCfg",
                type: "varchar(512) CHARACTER SET utf8mb4",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);
        }
    }
}
