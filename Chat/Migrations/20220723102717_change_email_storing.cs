using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chat.Migrations
{
    public partial class change_email_storing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "EmailUsername");

            migrationBuilder.AddColumn<string>(
                name: "EmailHostname",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailHostname",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "EmailUsername",
                table: "Users",
                newName: "Email");
        }
    }
}
