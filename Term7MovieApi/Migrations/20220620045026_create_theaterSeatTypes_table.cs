using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Term7MovieApi.Migrations
{
    public partial class create_theaterSeatTypes_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BonusPrice",
                table: "SeatTypes");

            migrationBuilder.CreateTable(
                name: "TheaterSeatTypes",
                columns: table => new
                {
                    TheaterId = table.Column<int>(type: "int", nullable: false),
                    SeatTypeId = table.Column<int>(type: "int", nullable: false),
                    BonusPrice = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheaterSeatTypes", x => new { x.TheaterId, x.SeatTypeId });
                    table.ForeignKey(
                        name: "FK_TheaterSeatTypes_SeatTypes_SeatTypeId",
                        column: x => x.SeatTypeId,
                        principalTable: "SeatTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TheaterSeatTypes_Theaters_TheaterId",
                        column: x => x.TheaterId,
                        principalTable: "Theaters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TheaterSeatTypes_SeatTypeId",
                table: "TheaterSeatTypes",
                column: "SeatTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TheaterSeatTypes");

            migrationBuilder.AddColumn<decimal>(
                name: "BonusPrice",
                table: "SeatTypes",
                type: "money",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
