using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adnc.Demo.Cust.Api.Migrations
{
    public partial class EventTraker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_eventtracker",
                table: "eventtracker");

            migrationBuilder.RenameTable(
                name: "eventtracker",
                newName: "cust_eventtracker");

            migrationBuilder.RenameIndex(
                name: "ix_eventtracker_eventid_trackername",
                table: "cust_eventtracker",
                newName: "ix_cust_eventtracker_eventid_trackername");

            migrationBuilder.AddPrimaryKey(
                name: "pk_cust_eventtracker",
                table: "cust_eventtracker",
                column: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_cust_eventtracker",
                table: "cust_eventtracker");

            migrationBuilder.RenameTable(
                name: "cust_eventtracker",
                newName: "eventtracker");

            migrationBuilder.RenameIndex(
                name: "ix_cust_eventtracker_eventid_trackername",
                table: "eventtracker",
                newName: "ix_eventtracker_eventid_trackername");

            migrationBuilder.AddPrimaryKey(
                name: "pk_eventtracker",
                table: "eventtracker",
                column: "id");
        }
    }
}
