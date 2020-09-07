using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityToDoList.Data.Migrations
{
    public partial class ToDoListUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserID",
                table: "ToDoList",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ToDoList_ApplicationUserID",
                table: "ToDoList",
                column: "ApplicationUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoList_AspNetUsers_ApplicationUserID",
                table: "ToDoList",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoList_AspNetUsers_ApplicationUserID",
                table: "ToDoList");

            migrationBuilder.DropIndex(
                name: "IX_ToDoList_ApplicationUserID",
                table: "ToDoList");

            migrationBuilder.DropColumn(
                name: "ApplicationUserID",
                table: "ToDoList");
        }
    }
}
