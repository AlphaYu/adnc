using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Adnc.Maint.Migrations.Migrations
{
    public partial class _2020101402 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SysLoginLog",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<long>(nullable: true),
                    Device = table.Column<string>(maxLength: 20, nullable: true),
                    Message = table.Column<string>(maxLength: 255, nullable: true),
                    Succeed = table.Column<bool>(nullable: false),
                    UserId = table.Column<long>(nullable: true),
                    Account = table.Column<string>(maxLength: 32, nullable: true),
                    UserName = table.Column<string>(maxLength: 64, nullable: true),
                    RemoteIpAddress = table.Column<string>(maxLength: 22, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysLoginLog", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SysLoginLog");
        }
    }
}
