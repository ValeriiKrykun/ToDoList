﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityToDoList.Migrations
{
    public partial class ApplicationUserIdString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoListData_AspNetUsers_ApplicationUsersId1",
                table: "TodoListData");

            migrationBuilder.DropIndex(
                name: "IX_TodoListData_ApplicationUsersId1",
                table: "TodoListData");

            migrationBuilder.DropColumn(
                name: "ApplicationUsersId1",
                table: "TodoListData");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUsersId",
                table: "TodoListData",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TodoListData_ApplicationUsersId",
                table: "TodoListData",
                column: "ApplicationUsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoListData_AspNetUsers_ApplicationUsersId",
                table: "TodoListData",
                column: "ApplicationUsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoListData_AspNetUsers_ApplicationUsersId",
                table: "TodoListData");

            migrationBuilder.DropIndex(
                name: "IX_TodoListData_ApplicationUsersId",
                table: "TodoListData");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationUsersId",
                table: "TodoListData",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUsersId1",
                table: "TodoListData",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TodoListData_ApplicationUsersId1",
                table: "TodoListData",
                column: "ApplicationUsersId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoListData_AspNetUsers_ApplicationUsersId1",
                table: "TodoListData",
                column: "ApplicationUsersId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
