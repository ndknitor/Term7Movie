using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Term7MovieApi.Migrations
{
    public partial class add_Theater_Transaction_relation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TheaterId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TheaterId",
                table: "Transactions",
                column: "TheaterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Theaters_TheaterId",
                table: "Transactions",
                column: "TheaterId",
                principalTable: "Theaters",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Theaters_TheaterId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_TheaterId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TheaterId",
                table: "Transactions");
        }
    }
}
