using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Term7MovieApi.Migrations
{
    public partial class add_constraint_room_ro_theaterid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MovieId",
                table: "TransactionHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_No_TheaterId",
                table: "Rooms",
                columns: new[] { "No", "TheaterId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Rooms_No_TheaterId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "TransactionHistories");
        }
    }
}
