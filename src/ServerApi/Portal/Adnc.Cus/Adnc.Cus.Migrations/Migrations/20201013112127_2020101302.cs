using Microsoft.EntityFrameworkCore.Migrations;

namespace Adnc.Cus.Migrations.Migrations
{
    public partial class _2020101302 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "CusTransactionLog",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200) CHARACTER SET utf8mb4",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "ExchangeType",
                table: "CusTransactionLog",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(3) CHARACTER SET utf8mb4",
                oldMaxLength: 3);

            migrationBuilder.AlterColumn<string>(
                name: "ExchageStatus",
                table: "CusTransactionLog",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(3) CHARACTER SET utf8mb4",
                oldMaxLength: 3);

            migrationBuilder.AlterColumn<string>(
                name: "Account",
                table: "CusTransactionLog",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(32) CHARACTER SET utf8mb4",
                oldMaxLength: 32);

            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                table: "CusTransactionLog",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "CusTransactionLog");

            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "CusTransactionLog",
                type: "varchar(200) CHARACTER SET utf8mb4",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExchangeType",
                table: "CusTransactionLog",
                type: "varchar(3) CHARACTER SET utf8mb4",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExchageStatus",
                table: "CusTransactionLog",
                type: "varchar(3) CHARACTER SET utf8mb4",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Account",
                table: "CusTransactionLog",
                type: "varchar(32) CHARACTER SET utf8mb4",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 32,
                oldNullable: true);
        }
    }
}
