using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adnc.Demo.Admin.Repository.Migrations;

/// <inheritdoc />
public partial class Update2025031201 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "params",
            table: "sys_menu",
            type: "varchar(128)",
            maxLength: 128,
            nullable: false,
            comment: "路由参数",
            oldClrType: typeof(string),
            oldType: "varchar(64)",
            oldMaxLength: 64,
            oldComment: "路由参数")
            .Annotation("MySql:CharSet", "utf8mb4 ")
            .OldAnnotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.AlterColumn<string>(
            name: "icon",
            table: "sys_menu",
            type: "varchar(32)",
            maxLength: 32,
            nullable: false,
            comment: "图标",
            oldClrType: typeof(string),
            oldType: "varchar(16)",
            oldMaxLength: 16,
            oldComment: "图标")
            .Annotation("MySql:CharSet", "utf8mb4 ")
            .OldAnnotation("MySql:CharSet", "utf8mb4 ");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "params",
            table: "sys_menu",
            type: "varchar(64)",
            maxLength: 64,
            nullable: false,
            comment: "路由参数",
            oldClrType: typeof(string),
            oldType: "varchar(128)",
            oldMaxLength: 128,
            oldComment: "路由参数")
            .Annotation("MySql:CharSet", "utf8mb4 ")
            .OldAnnotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.AlterColumn<string>(
            name: "icon",
            table: "sys_menu",
            type: "varchar(16)",
            maxLength: 16,
            nullable: false,
            comment: "图标",
            oldClrType: typeof(string),
            oldType: "varchar(32)",
            oldMaxLength: 32,
            oldComment: "图标")
            .Annotation("MySql:CharSet", "utf8mb4 ")
            .OldAnnotation("MySql:CharSet", "utf8mb4 ");
    }
}
