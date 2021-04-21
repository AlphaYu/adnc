using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Adnc.Usr.Migrations.Migrations
{
    public partial class _202101311 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SysUserFinance_SysUser_ID",
                table: "SysUserFinance");

            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "SysRelation");

            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "SysRelation");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "SysUserFinance",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "SysUser",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "SysRole",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "SysRelation",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "SysMenu",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "SysDept",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SysUserFinance_SysUser_Id",
                table: "SysUserFinance",
                column: "Id",
                principalTable: "SysUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SysUserFinance_SysUser_Id",
                table: "SysUserFinance");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "SysUserFinance",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "SysUser",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "SysRole",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "SysRelation",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "SysMenu",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "SysDept",
                newName: "ID");

            migrationBuilder.AddColumn<long>(
                name: "CreateBy",
                table: "SysRelation",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTime",
                table: "SysRelation",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SysUserFinance_SysUser_ID",
                table: "SysUserFinance",
                column: "ID",
                principalTable: "SysUser",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
