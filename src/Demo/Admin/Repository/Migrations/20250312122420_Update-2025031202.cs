using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adnc.Demo.Admin.Repository.Migrations;

/// <inheritdoc />
public partial class Update2025031202 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<bool>(
            name: "status",
            table: "sys_role",
            type: "tinyint(1)",
            nullable: false,
            comment: "",
            oldClrType: typeof(int),
            oldType: "int",
            oldComment: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
            name: "status",
            table: "sys_role",
            type: "int",
            nullable: false,
            comment: "",
            oldClrType: typeof(bool),
            oldType: "tinyint(1)",
            oldComment: "");
    }
}
