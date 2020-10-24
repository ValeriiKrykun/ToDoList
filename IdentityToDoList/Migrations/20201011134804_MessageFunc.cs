using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityToDoList.Migrations
{
    public partial class MessageFunc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "TodoListData",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "TodoListData");
        }
    }
}
