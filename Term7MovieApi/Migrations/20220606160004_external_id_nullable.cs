using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Term7MovieApi.Migrations
{
    public partial class external_id_nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Movies_ExternalId",
                table: "Movies");

            migrationBuilder.AlterColumn<int>(
                name: "ExternalId",
                table: "Movies",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ExternalId",
                table: "Movies",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movies_ExternalId",
                table: "Movies",
                column: "ExternalId",
                unique: true);
        }
    }
}
