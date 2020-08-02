using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace  Adnc.Infr.EfCore.Migrations
{
    public partial class userfinance2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SysUserFinance",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false),
                    CreateBy = table.Column<long>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<long>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    RowVersion = table.Column<DateTime>(type: "timestamp(3)", nullable: false, defaultValueSql: "'0000-00-00 00:00:00.000'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysUserFinance", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SysUserFinance_SysUser_ID",
                        column: x => x.ID,
                        principalTable: "SysUser",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SysUserFinance");
        }
    }
}
