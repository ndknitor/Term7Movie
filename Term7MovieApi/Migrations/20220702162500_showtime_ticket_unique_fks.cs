using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Term7MovieApi.Migrations
{
    public partial class showtime_ticket_unique_fks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShowtimeTicketTypes_ShowtimeId",
                table: "ShowtimeTicketTypes");

            migrationBuilder.CreateIndex(
                name: "IX_TicketTypes_Name_CompanyId",
                table: "TicketTypes",
                columns: new[] { "Name", "CompanyId" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ShowtimeTicketTypes_ShowtimeId_TicketTypeId",
                table: "ShowtimeTicketTypes",
                columns: new[] { "ShowtimeId", "TicketTypeId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TicketTypes_Name_CompanyId",
                table: "TicketTypes");

            migrationBuilder.DropIndex(
                name: "IX_ShowtimeTicketTypes_ShowtimeId_TicketTypeId",
                table: "ShowtimeTicketTypes");

            migrationBuilder.CreateIndex(
                name: "IX_ShowtimeTicketTypes_ShowtimeId",
                table: "ShowtimeTicketTypes",
                column: "ShowtimeId");
        }
    }
}
