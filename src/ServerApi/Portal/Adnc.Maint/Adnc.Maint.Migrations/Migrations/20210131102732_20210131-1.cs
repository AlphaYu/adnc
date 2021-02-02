using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Adnc.Maint.Migrations.Migrations
{
    public partial class _202101311 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SysLoginLog");

            migrationBuilder.DropTable(
                name: "SysTask");

            migrationBuilder.DropTable(
                name: "SysTaskLog");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "SysNotice",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "SysDict",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "SysCfg",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "SysNotice",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "SysDict",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "SysCfg",
                newName: "ID");

            migrationBuilder.CreateTable(
                name: "SysLoginLog",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Account = table.Column<string>(type: "varchar(16) CHARACTER SET utf8mb4", maxLength: 16, nullable: true),
                    CreateBy = table.Column<long>(type: "bigint", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Device = table.Column<string>(type: "varchar(16) CHARACTER SET utf8mb4", maxLength: 16, nullable: true),
                    Message = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    RemoteIpAddress = table.Column<string>(type: "varchar(15) CHARACTER SET utf8mb4", maxLength: 15, nullable: true),
                    Succeed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    UserName = table.Column<string>(type: "varchar(16) CHARACTER SET utf8mb4", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysLoginLog", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SysTask",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Concurrent = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    CreateBy = table.Column<long>(type: "bigint", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Cron = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: true),
                    Data = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", maxLength: 65535, nullable: true),
                    Disabled = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    ExecAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ExecResult = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", maxLength: 65535, nullable: true),
                    JobClass = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    JobGroup = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: true),
                    ModifyBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifyTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Name = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: true),
                    Note = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysTask", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SysTaskLog",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<long>(type: "bigint", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ExecAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ExecSuccess = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    IdTask = table.Column<long>(type: "bigint", nullable: true),
                    JobException = table.Column<string>(type: "varchar(500) CHARACTER SET utf8mb4", maxLength: 500, nullable: true),
                    Name = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysTaskLog", x => x.ID);
                });
        }
    }
}
