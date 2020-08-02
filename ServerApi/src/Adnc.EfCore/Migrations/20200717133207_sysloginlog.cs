using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace  Adnc.Infr.EfCore.Migrations
{
    public partial class sysloginlog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SysOperationLog");

            migrationBuilder.DropColumn(
                name: "LoginName",
                table: "SysLoginLog");

            migrationBuilder.AlterColumn<bool>(
                name: "Succeed",
                table: "SysLoginLog",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255) CHARACTER SET utf8mb4",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ID",
                table: "SysLoginLog",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "Account",
                table: "SysLoginLog",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Device",
                table: "SysLoginLog",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RemoteIpAddress",
                table: "SysLoginLog",
                maxLength: 22,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "SysLoginLog",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Account",
                table: "SysLoginLog");

            migrationBuilder.DropColumn(
                name: "Device",
                table: "SysLoginLog");

            migrationBuilder.DropColumn(
                name: "RemoteIpAddress",
                table: "SysLoginLog");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "SysLoginLog");

            migrationBuilder.AlterColumn<string>(
                name: "Succeed",
                table: "SysLoginLog",
                type: "varchar(255) CHARACTER SET utf8mb4",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "SysLoginLog",
                type: "int",
                nullable: false,
                oldClrType: typeof(long))
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "LoginName",
                table: "SysLoginLog",
                type: "varchar(255) CHARACTER SET utf8mb4",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SysOperationLog",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Account = table.Column<string>(type: "varchar(32) CHARACTER SET utf8mb4", maxLength: 32, nullable: true),
                    ClassName = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    LogName = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    LogType = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    Message = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", maxLength: 65535, nullable: true),
                    Method = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    RemoteIpAddress = table.Column<string>(type: "varchar(22) CHARACTER SET utf8mb4", maxLength: 22, nullable: true),
                    Succeed = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    UserName = table.Column<string>(type: "varchar(64) CHARACTER SET utf8mb4", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysOperationLog", x => x.ID);
                });
        }
    }
}
