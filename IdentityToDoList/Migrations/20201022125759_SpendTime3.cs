﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityToDoList.Migrations
{
    public partial class SpendTime3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Start",
                table: "TodoListData",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Stop",
                table: "TodoListData",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Start",
                table: "TodoListData");

            migrationBuilder.DropColumn(
                name: "Stop",
                table: "TodoListData");
        }
    }
}
