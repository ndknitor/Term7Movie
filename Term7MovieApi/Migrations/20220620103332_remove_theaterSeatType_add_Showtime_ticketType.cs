using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Term7MovieApi.Migrations
{
    public partial class remove_theaterSeatType_add_Showtime_ticketType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Showtimes_ShowTimeId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "TheaterSeatTypes");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_ShowTimeId",
                table: "Tickets");

            migrationBuilder.AlterColumn<long>(
                name: "ShowTimeId",
                table: "Tickets",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ShowtimeTicketTypeId",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TicketType",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShowtimeTicketTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShowtimeId = table.Column<long>(type: "bigint", nullable: false),
                    TicketTypeId = table.Column<long>(type: "bigint", nullable: false),
                    OriginalPrice = table.Column<decimal>(type: "money", nullable: false),
                    ReceivePrice = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowtimeTicketTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShowtimeTicketTypes_Showtimes_ShowtimeId",
                        column: x => x.ShowtimeId,
                        principalTable: "Showtimes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShowtimeTicketTypes_TicketType_TicketTypeId",
                        column: x => x.TicketTypeId,
                        principalTable: "TicketType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ShowtimeTicketTypeId",
                table: "Tickets",
                column: "ShowtimeTicketTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ShowtimeTicketTypes_ShowtimeId",
                table: "ShowtimeTicketTypes",
                column: "ShowtimeId");

            migrationBuilder.CreateIndex(
                name: "IX_ShowtimeTicketTypes_TicketTypeId",
                table: "ShowtimeTicketTypes",
                column: "TicketTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_ShowtimeTicketTypes_ShowtimeTicketTypeId",
                table: "Tickets",
                column: "ShowtimeTicketTypeId",
                principalTable: "ShowtimeTicketTypes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_ShowtimeTicketTypes_ShowtimeTicketTypeId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "ShowtimeTicketTypes");

            migrationBuilder.DropTable(
                name: "TicketType");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_ShowtimeTicketTypeId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ShowtimeTicketTypeId",
                table: "Tickets");

            migrationBuilder.AlterColumn<long>(
                name: "ShowTimeId",
                table: "Tickets",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

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
                name: "IX_Tickets_ShowTimeId",
                table: "Tickets",
                column: "ShowTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_TheaterSeatTypes_SeatTypeId",
                table: "TheaterSeatTypes",
                column: "SeatTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Showtimes_ShowTimeId",
                table: "Tickets",
                column: "ShowTimeId",
                principalTable: "Showtimes",
                principalColumn: "Id");
        }
    }
}
