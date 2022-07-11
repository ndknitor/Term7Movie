using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Term7MovieApi.Migrations
{
    public partial class add_showtimes_theaterId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TheaterId",
                table: "Showtimes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Showtimes_TheaterId",
                table: "Showtimes",
                column: "TheaterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Showtimes_Theaters_TheaterId",
                table: "Showtimes",
                column: "TheaterId",
                principalTable: "Theaters",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Showtimes_Theaters_TheaterId",
                table: "Showtimes");

            migrationBuilder.DropIndex(
                name: "IX_Showtimes_TheaterId",
                table: "Showtimes");

            migrationBuilder.DropColumn(
                name: "TheaterId",
                table: "Showtimes");
        }
    }
}
