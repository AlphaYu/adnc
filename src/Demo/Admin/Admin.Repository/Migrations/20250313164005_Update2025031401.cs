using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adnc.Demo.Admin.Repository.Migrations;

/// <inheritdoc />
public partial class Update2025031401 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "dictdata",
            columns: table => new
            {
                id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                createby = table.Column<long>(type: "bigint", nullable: false, comment: "创建人"),
                createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间/注册时间"),
                modifyby = table.Column<long>(type: "bigint", nullable: false, comment: "最后更新人"),
                modifytime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "最后更新时间"),
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
                table.PrimaryKey("pk_dictdata", x => x.id);
            },
            comment: "字典数据")
            .Annotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.CreateTable(
            name: "sys_config",
            columns: table => new
            {
                id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                createby = table.Column<long>(type: "bigint", nullable: false, comment: "创建人"),
                createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间/注册时间"),
                modifyby = table.Column<long>(type: "bigint", nullable: false, comment: "最后更新人"),
                modifytime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "最后更新时间"),
                key = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false, comment: "参数键")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                name = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false, comment: "参数名")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                value = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false, comment: "参数值")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                remark = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false, comment: "备注")
                    .Annotation("MySql:CharSet", "utf8mb4 ")
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_sys_config", x => x.id);
            },
            comment: "系统参数")
            .Annotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.CreateTable(
            name: "sys_dictionary",
            columns: table => new
            {
                id = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                createby = table.Column<long>(type: "bigint", nullable: false, comment: "创建人"),
                createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间/注册时间"),
                modifyby = table.Column<long>(type: "bigint", nullable: false, comment: "最后更新人"),
                modifytime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "最后更新时间"),
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
            comment: "字典")
            .Annotation("MySql:CharSet", "utf8mb4 ");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "dictdata");

        migrationBuilder.DropTable(
            name: "sys_config");

        migrationBuilder.DropTable(
            name: "sys_dictionary");
    }
}
