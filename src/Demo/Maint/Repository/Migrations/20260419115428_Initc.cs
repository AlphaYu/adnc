using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adnc.Demo.Maint.Repository.Migrations;

/// <inheritdoc />
public partial class Initc : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.CreateTable(
            name: "maint_eventtracker",
            columns: table => new
            {
                id = table.Column<long>(type: "bigint", nullable: false, comment: "")
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                eventid = table.Column<long>(type: "bigint", nullable: false, comment: ""),
                trackername = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "")
                    .Annotation("MySql:CharSet", "utf8mb4 "),
                createby = table.Column<long>(type: "bigint", nullable: false, comment: "Creator"),
                createtime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "Creation time / registration time")
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_maint_eventtracker", x => x.id);
            },
            comment: "Event tracking/processing information")
            .Annotation("MySql:CharSet", "utf8mb4 ");

        migrationBuilder.CreateIndex(
            name: "ix_maint_eventtracker_eventid_trackername",
            table: "maint_eventtracker",
            columns: new[] { "eventid", "trackername" },
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "maint_eventtracker");
    }
}
