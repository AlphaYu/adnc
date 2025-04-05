using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adnc.Demo.Admin.Repository.Migrations;

/// <inheritdoc />
public partial class Update2025031601 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "pk_dictdata",
            table: "dictdata");

        migrationBuilder.RenameTable(
            name: "dictdata",
            newName: "sys_dictionary_data");

        migrationBuilder.AlterColumn<string>(
            name: "name",
            table: "sys_user",
            type: "varchar(32)",
            maxLength: 32,
            nullable: false,
            comment: "姓名",
            oldClrType: typeof(string),
            oldType: "varchar(16)",
            oldMaxLength: 16,
            oldComment: "姓名")
            .Annotation("MySql:CharSet", "utf8mb4 ")
            .OldAnnotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.AlterColumn<string>(
            name: "avatar",
            table: "sys_user",
            type: "varchar(128)",
            maxLength: 128,
            nullable: false,
            comment: "头像路径",
            oldClrType: typeof(string),
            oldType: "varchar(64)",
            oldMaxLength: 64,
            oldComment: "头像路径")
            .Annotation("MySql:CharSet", "utf8mb4 ")
            .OldAnnotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.AlterColumn<string>(
            name: "account",
            table: "sys_user",
            type: "varchar(32)",
            maxLength: 32,
            nullable: false,
            comment: "账号",
            oldClrType: typeof(string),
            oldType: "varchar(16)",
            oldMaxLength: 16,
            oldComment: "账号")
            .Annotation("MySql:CharSet", "utf8mb4 ")
            .OldAnnotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.AddPrimaryKey(
            name: "pk_sys_dictionary_data",
            table: "sys_dictionary_data",
            column: "id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "pk_sys_dictionary_data",
            table: "sys_dictionary_data");

        migrationBuilder.RenameTable(
            name: "sys_dictionary_data",
            newName: "dictdata");

        migrationBuilder.AlterColumn<string>(
            name: "name",
            table: "sys_user",
            type: "varchar(16)",
            maxLength: 16,
            nullable: false,
            comment: "姓名",
            oldClrType: typeof(string),
            oldType: "varchar(32)",
            oldMaxLength: 32,
            oldComment: "姓名")
            .Annotation("MySql:CharSet", "utf8mb4 ")
            .OldAnnotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.AlterColumn<string>(
            name: "avatar",
            table: "sys_user",
            type: "varchar(64)",
            maxLength: 64,
            nullable: false,
            comment: "头像路径",
            oldClrType: typeof(string),
            oldType: "varchar(128)",
            oldMaxLength: 128,
            oldComment: "头像路径")
            .Annotation("MySql:CharSet", "utf8mb4 ")
            .OldAnnotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.AlterColumn<string>(
            name: "account",
            table: "sys_user",
            type: "varchar(16)",
            maxLength: 16,
            nullable: false,
            comment: "账号",
            oldClrType: typeof(string),
            oldType: "varchar(32)",
            oldMaxLength: 32,
            oldComment: "账号")
            .Annotation("MySql:CharSet", "utf8mb4 ")
            .OldAnnotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.AddPrimaryKey(
            name: "pk_dictdata",
            table: "dictdata",
            column: "id");
    }
}
