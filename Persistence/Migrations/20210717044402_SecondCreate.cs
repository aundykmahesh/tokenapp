using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class SecondCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tokens_AspNetUsers_GeneratedById",
                table: "Tokens");

            migrationBuilder.DropIndex(
                name: "IX_Tokens_GeneratedById",
                table: "Tokens");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Tokens");

            migrationBuilder.RenameColumn(
                name: "GeneratedById",
                table: "Tokens",
                newName: "AppUrl");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Tokens",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Tokens");

            migrationBuilder.RenameColumn(
                name: "AppUrl",
                table: "Tokens",
                newName: "GeneratedById");

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "Tokens",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_GeneratedById",
                table: "Tokens",
                column: "GeneratedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Tokens_AspNetUsers_GeneratedById",
                table: "Tokens",
                column: "GeneratedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
