using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adnc.Demo.Usr.Repository.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4 ");

            migrationBuilder.CreateTable(
                name: "sys_eventtracker",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
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
                    code = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, comment: "编号")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    component = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true, comment: "組件配置")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    hidden = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "是否隐藏"),
                    icon = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true, comment: "图标")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    ismenu = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "是否是菜单1:菜单,0:按钮"),
                    isopen = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "是否默认打开1:是,0:否"),
                    levels = table.Column<int>(type: "int", nullable: false, comment: "级别"),
                    name = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, comment: "名称")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    ordinal = table.Column<int>(type: "int", nullable: false, comment: "序号"),
                    pcode = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, comment: "父菜单编号")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    pcodes = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false, comment: "递归父级菜单编号")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    status = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "状态1:启用,0:禁用"),
                    tips = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true, comment: "鼠标悬停提示信息")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    url = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false, comment: "链接")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    createby = table.Column<long>(type: "bigint", nullable: false, comment: "创建人"),
                    createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间/注册时间"),
                    modifyby = table.Column<long>(type: "bigint", nullable: true, comment: "最后更新人"),
                    modifytime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "最后更新时间")
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
                    fullname = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    ordinal = table.Column<int>(type: "int", nullable: false, comment: ""),
                    pid = table.Column<long>(type: "bigint", nullable: true, comment: ""),
                    pids = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false, comment: "")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    simplename = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, comment: "")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    tips = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true, comment: "")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    createby = table.Column<long>(type: "bigint", nullable: false, comment: "创建人"),
                    createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间/注册时间"),
                    modifyby = table.Column<long>(type: "bigint", nullable: true, comment: "最后更新人"),
                    modifytime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "最后更新时间")
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
                    deptid = table.Column<long>(type: "bigint", nullable: true, comment: ""),
                    name = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    ordinal = table.Column<int>(type: "int", nullable: false, comment: ""),
                    pid = table.Column<long>(type: "bigint", nullable: true, comment: ""),
                    tips = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true, comment: "")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    createby = table.Column<long>(type: "bigint", nullable: false, comment: "创建人"),
                    createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间/注册时间"),
                    modifyby = table.Column<long>(type: "bigint", nullable: true, comment: "最后更新人"),
                    modifytime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "最后更新时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sys_role", x => x.id);
                },
                comment: "角色")
                .Annotation("MySql:CharSet", "utf8mb4 ");

            migrationBuilder.CreateTable(
                name: "sys_rolerelation",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                    menuid = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                    roleid = table.Column<long>(type: "bigint", nullable: false, comment: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sys_rolerelation", x => x.id);
                    table.ForeignKey(
                        name: "fk_sys_rolerelation_sys_menu_menuid",
                        column: x => x.menuid,
                        principalTable: "sys_menu",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "菜单角色关系")
                .Annotation("MySql:CharSet", "utf8mb4 ");

            migrationBuilder.CreateTable(
                name: "sys_user",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                    isdeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false, comment: ""),
                    account = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, comment: "账号")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    avatar = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false, comment: "头像路径")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    birthday = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "生日"),
                    deptid = table.Column<long>(type: "bigint", nullable: true, comment: "部门Id"),
                    email = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "email")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    name = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, comment: "姓名")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    password = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false, comment: "密码")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    phone = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false, comment: "手机号")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    roleids = table.Column<string>(type: "varchar(72)", maxLength: 72, nullable: false, comment: "角色id列表，以逗号分隔")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    salt = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: false, comment: "密码盐")
                        .Annotation("MySql:CharSet", "utf8mb4 "),
                    sex = table.Column<int>(type: "int", nullable: false, comment: "性别"),
                    status = table.Column<int>(type: "int", nullable: false, comment: "状态"),
                    createby = table.Column<long>(type: "bigint", nullable: false, comment: "创建人"),
                    createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间/注册时间"),
                    modifyby = table.Column<long>(type: "bigint", nullable: true, comment: "最后更新人"),
                    modifytime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "最后更新时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sys_user", x => x.id);
                    table.ForeignKey(
                        name: "fk_sys_user_sys_organization_deptid",
                        column: x => x.deptid,
                        principalTable: "sys_organization",
                        principalColumn: "id");
                },
                comment: "管理员")
                .Annotation("MySql:CharSet", "utf8mb4 ");

            migrationBuilder.CreateIndex(
                name: "ix_sys_eventtracker_eventid_trackername",
                table: "sys_eventtracker",
                columns: new[] { "eventid", "trackername" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sys_rolerelation_menuid",
                table: "sys_rolerelation",
                column: "menuid");

            migrationBuilder.CreateIndex(
                name: "ix_sys_user_deptid",
                table: "sys_user",
                column: "deptid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sys_eventtracker");

            migrationBuilder.DropTable(
                name: "sys_role");

            migrationBuilder.DropTable(
                name: "sys_rolerelation");

            migrationBuilder.DropTable(
                name: "sys_user");

            migrationBuilder.DropTable(
                name: "sys_menu");

            migrationBuilder.DropTable(
                name: "sys_organization");
        }
    }
}
