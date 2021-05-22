using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Adnc.Maint.Migrations.Migrations
{
    public partial class _2021020801 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SysCfg",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<long>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<long>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 64, nullable: true),
                    Value = table.Column<string>(maxLength: 128, nullable: true),
                    Description = table.Column<string>(maxLength: 256, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysCfg", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysDict",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<long>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<long>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 16, nullable: true),
                    Ordinal = table.Column<int>(nullable: false),
                    Pid = table.Column<long>(nullable: false),
                    Tips = table.Column<string>(maxLength: 64, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysDict", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysNotice",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<long>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<long>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Content = table.Column<string>(maxLength: 255, nullable: true),
                    Title = table.Column<string>(maxLength: 255, nullable: true),
                    Type = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysNotice", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SysCfg");

            migrationBuilder.DropTable(
                name: "SysDict");

            migrationBuilder.DropTable(
                name: "SysNotice");
        }
    }
}