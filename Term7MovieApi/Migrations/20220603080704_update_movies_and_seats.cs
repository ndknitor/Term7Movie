using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Term7MovieApi.Migrations
{
    public partial class update_movies_and_seats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Seats_RoomId",
                table: "Seats");

            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                table: "Theaters",
                type: "varchar(10)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Longitude",
                table: "Theaters",
                type: "varchar(10)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Seats",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Seats_RoomId_RowPos_ColumnPos",
                table: "Seats",
                columns: new[] { "RoomId", "RowPos", "ColumnPos" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Seats_RoomId_RowPos_ColumnPos",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Theaters");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Theaters");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Seats");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_RoomId",
                table: "Seats",
                column: "RoomId");
        }
    }
}
