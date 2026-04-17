using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adnc.Demo.Admin.Repository.Migrations;

/// <inheritdoc />
public partial class Update2025031203 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<bool>(
            name: "status",
            table: "sys_user",
            type: "tinyint(1)",
            nullable: false,
            comment: "状态",
            oldClrType: typeof(int),
            oldType: "int",
            oldComment: "状态");

        migrationBuilder.AlterColumn<bool>(
            name: "isdeleted",
            table: "sys_user",
            type: "tinyint(1)",
            nullable: false,
            defaultValue: false,
            comment: "删除标识",
            oldClrType: typeof(bool),
            oldType: "tinyint(1)",
            oldDefaultValue: false,
            oldComment: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
            name: "status",
            table: "sys_user",
            type: "int",
            nullable: false,
            comment: "状态",
            oldClrType: typeof(bool),
            oldType: "tinyint(1)",
            oldComment: "状态");

        migrationBuilder.AlterColumn<bool>(
            name: "isdeleted",
            table: "sys_user",
            type: "tinyint(1)",
            nullable: false,
            defaultValue: false,
            comment: "",
            oldClrType: typeof(bool),
            oldType: "tinyint(1)",
            oldDefaultValue: false,
            oldComment: "删除标识");
    }
}
