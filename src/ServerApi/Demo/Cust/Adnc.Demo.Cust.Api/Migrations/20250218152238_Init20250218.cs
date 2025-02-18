using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adnc.Demo.Cust.Api.Migrations
{
    /// <inheritdoc />
    public partial class Init20250218 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "cust_eventtracker",
                type: "bigint",
                nullable: false,
                comment: "",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("Relational:ColumnOrder", 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "cust_eventtracker",
                type: "bigint",
                nullable: false,
                comment: "",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "")
                .Annotation("Relational:ColumnOrder", 1)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);
        }
    }
}
