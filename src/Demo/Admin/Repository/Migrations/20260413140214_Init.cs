using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adnc.Demo.Admin.Repository.Migrations;

/// <inheritdoc />
public partial class Init : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.CreateTable(
            name: "sys_config",
            columns: table => new
            {
                id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                createby = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: ""),
                modifyby = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                modifytime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: ""),
                key = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false, comment: "Parameter key")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                name = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false, comment: "Parameter name")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                value = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false, comment: "Parameter value")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                remark = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false, comment: "Remark")
                    .Annotation("MySql:CharSet", "utf8mb4 ")
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_sys_config", x => x.id);
            },
            comment: "System parameter")
            .Annotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.CreateTable(
            name: "sys_dictionary",
            columns: table => new
            {
                id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                createby = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: ""),
                modifyby = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                modifytime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: ""),
                code = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                name = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                remark = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false, comment: "")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                status = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "")
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_sys_dictionary", x => x.id);
            },
            comment: "Dictionary")
            .Annotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.CreateTable(
            name: "sys_dictionary_data",
            columns: table => new
            {
                id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                createby = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: ""),
                modifyby = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                modifytime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: ""),
                dictcode = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                label = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                value = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                tagtype = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                status = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: ""),
                ordinal = table.Column<int>(type: "int", nullable: false, comment: "")
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_sys_dictionary_data", x => x.id);
            },
            comment: "Dictionary data")
            .Annotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.CreateTable(
            name: "sys_eventtracker",
            columns: table => new
            {
                id = table.Column<long>(type: "bigint", nullable: false, comment: "")
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                eventid = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                trackername = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                createby = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "")
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_sys_eventtracker", x => x.id);
            },
            comment: "")
            .Annotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.CreateTable(
            name: "sys_menu",
            columns: table => new
            {
                id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                createby = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: ""),
                modifyby = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                modifytime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: ""),
                parentid = table.Column<long>(type: "bigint", nullable: false, comment: "Parent menu ID"),
                parentids = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false, comment: "Parent menu ID path")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                name = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "Name")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                perm = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "Permission code")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                routename = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false, comment: "Route name")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                routepath = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false, comment: "Route path")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                type = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, comment: "Menu type")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                component = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false, comment: "Component configuration")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                visible = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "Visible"),
                redirect = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false, comment: "Redirect route path")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                icon = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "Icon")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                keepalive = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "Enable page caching"),
                alwaysshow = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "Always show when there is only one child route"),
                @params = table.Column<string>(name: "params", type: "varchar(128)", maxLength: 128, nullable: false, comment: "Route parameters")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                ordinal = table.Column<int>(type: "int", nullable: false, comment: "Ordinal")
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_sys_menu", x => x.id);
            },
            comment: "Menu")
            .Annotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.CreateTable(
            name: "sys_organization",
            columns: table => new
            {
                id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                createby = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: ""),
                modifyby = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                modifytime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: ""),
                parentid = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                parentids = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false, comment: "")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                code = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, comment: "")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                name = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                status = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: ""),
                ordinal = table.Column<int>(type: "int", nullable: false, comment: "")
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_sys_organization", x => x.id);
            },
            comment: "Department")
            .Annotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.CreateTable(
            name: "sys_role",
            columns: table => new
            {
                id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                createby = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: ""),
                modifyby = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                modifytime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: ""),
                name = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                code = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                datascope = table.Column<int>(type: "int", nullable: false, comment: ""),
                status = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: ""),
                ordinal = table.Column<int>(type: "int", nullable: false, comment: "")
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_sys_role", x => x.id);
            },
            comment: "Role")
            .Annotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.CreateTable(
            name: "sys_role_menu_relation",
            columns: table => new
            {
                id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                menuid = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                roleid = table.Column<long>(type: "bigint", nullable: false, comment: "")
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_sys_role_menu_relation", x => x.id);
            },
            comment: "Menu-role relation")
            .Annotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.CreateTable(
            name: "sys_role_user_relation",
            columns: table => new
            {
                id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                userid = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                roleid = table.Column<long>(type: "bigint", nullable: false, comment: "")
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_sys_role_user_relation", x => x.id);
            },
            comment: "User-role relation")
            .Annotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.CreateTable(
            name: "sys_user",
            columns: table => new
            {
                id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                isdeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false, comment: "Deletion flag"),
                createby = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: ""),
                modifyby = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                modifytime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: ""),
                account = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "Account")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                avatar = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false, comment: "Avatar path")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                birthday = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "Birthday"),
                deptid = table.Column<long>(type: "bigint", nullable: false, comment: "Department ID"),
                email = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "email")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                name = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "Name")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                password = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "Password")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                mobile = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false, comment: "Mobile number")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                salt = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: false, comment: "Password salt")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                gender = table.Column<int>(type: "int", nullable: false, comment: "Gender"),
                status = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "Status")
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_sys_user", x => x.id);
            },
            comment: "Administrator")
            .Annotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.CreateIndex(
            name: "ix_sys_eventtracker_eventid_trackername",
            table: "sys_eventtracker",
            columns: ["eventid", "trackername"],
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "sys_config");

        migrationBuilder.DropTable(
            name: "sys_dictionary");

        migrationBuilder.DropTable(
            name: "sys_dictionary_data");

        migrationBuilder.DropTable(
            name: "sys_eventtracker");

        migrationBuilder.DropTable(
            name: "sys_menu");

        migrationBuilder.DropTable(
            name: "sys_organization");

        migrationBuilder.DropTable(
            name: "sys_role");

        migrationBuilder.DropTable(
            name: "sys_role_menu_relation");

        migrationBuilder.DropTable(
            name: "sys_role_user_relation");

        migrationBuilder.DropTable(
            name: "sys_user");
    }
}
