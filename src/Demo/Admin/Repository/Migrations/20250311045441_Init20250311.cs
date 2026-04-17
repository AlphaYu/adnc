using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adnc.Demo.Admin.Repository.Migrations;

/// <inheritdoc />
public partial class Init20250311 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
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
                createby = table.Column<long>(type: "bigint", nullable: false, comment: "创建人"),
                createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间/注册时间")
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_sys_eventtracker", x => x.id);
            },
            comment: "事件跟踪/处理信息")
            .Annotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.CreateTable(
            name: "sys_menu",
            columns: table => new
            {
                id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                createby = table.Column<long>(type: "bigint", nullable: false, comment: "创建人"),
                createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间/注册时间"),
                modifyby = table.Column<long>(type: "bigint", nullable: false, comment: "最后更新人"),
                modifytime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "最后更新时间"),
                parentid = table.Column<long>(type: "bigint", nullable: false, comment: "父菜单Id"),
                parentids = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false, comment: "父菜单Id组合")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                name = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "名称")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                perm = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "权限编码")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                routename = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false, comment: "路由名称")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                routepath = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false, comment: "路由路径")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                type = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, comment: "菜单类型")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                component = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false, comment: "組件配置")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                visible = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "是否显示"),
                redirect = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false, comment: "跳转路由路径")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                icon = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, comment: "图标")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                keepalive = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "是否开启页面缓存"),
                alwaysshow = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "只有一个子路由是否始终显示"),
                @params = table.Column<string>(name: "params", type: "varchar(64)", maxLength: 64, nullable: false, comment: "路由参数")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                ordinal = table.Column<int>(type: "int", nullable: false, comment: "序号")
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_sys_menu", x => x.id);
            },
            comment: "菜单")
            .Annotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.CreateTable(
            name: "sys_organization",
            columns: table => new
            {
                id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                createby = table.Column<long>(type: "bigint", nullable: false, comment: "创建人"),
                createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间/注册时间"),
                modifyby = table.Column<long>(type: "bigint", nullable: false, comment: "最后更新人"),
                modifytime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "最后更新时间"),
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
            comment: "部门")
            .Annotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.CreateTable(
            name: "sys_role",
            columns: table => new
            {
                id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                createby = table.Column<long>(type: "bigint", nullable: false, comment: "创建人"),
                createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间/注册时间"),
                modifyby = table.Column<long>(type: "bigint", nullable: false, comment: "最后更新人"),
                modifytime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "最后更新时间"),
                name = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                code = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                datascope = table.Column<int>(type: "int", nullable: false, comment: ""),
                status = table.Column<int>(type: "int", nullable: false, comment: ""),
                ordinal = table.Column<int>(type: "int", nullable: false, comment: "")
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_sys_role", x => x.id);
            },
            comment: "角色")
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
            comment: "菜单角色关系")
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
            comment: "用户角色关系")
            .Annotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.CreateTable(
            name: "sys_user",
            columns: table => new
            {
                id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                isdeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false, comment: ""),
                createby = table.Column<long>(type: "bigint", nullable: false, comment: "创建人"),
                createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间/注册时间"),
                modifyby = table.Column<long>(type: "bigint", nullable: false, comment: "最后更新人"),
                modifytime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "最后更新时间"),
                account = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, comment: "账号")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                avatar = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false, comment: "头像路径")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                birthday = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "生日"),
                deptid = table.Column<long>(type: "bigint", nullable: false, comment: "部门Id"),
                email = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "email")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                name = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, comment: "姓名")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                password = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "密码")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                mobile = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false, comment: "手机号")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                salt = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: false, comment: "密码盐")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                gender = table.Column<int>(type: "int", nullable: false, comment: "性别"),
                status = table.Column<int>(type: "int", nullable: false, comment: "状态")
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_sys_user", x => x.id);
            },
            comment: "管理员")
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
