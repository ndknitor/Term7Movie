using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Term7MovieApi.Migrations
{
    public partial class add_color_to_category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShowtimeTicketTypes_TicketType_TicketTypeId",
                table: "ShowtimeTicketTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketType",
                table: "TicketType");

            migrationBuilder.RenameTable(
                name: "TicketType",
                newName: "TicketTypes");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketTypes",
                table: "TicketTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShowtimeTicketTypes_TicketTypes_TicketTypeId",
                table: "ShowtimeTicketTypes",
                column: "TicketTypeId",
                principalTable: "TicketTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShowtimeTicketTypes_TicketTypes_TicketTypeId",
                table: "ShowtimeTicketTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketTypes",
                table: "TicketTypes");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "TicketTypes",
                newName: "TicketType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketType",
                table: "TicketType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShowtimeTicketTypes_TicketType_TicketTypeId",
                table: "ShowtimeTicketTypes",
                column: "TicketTypeId",
                principalTable: "TicketType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
