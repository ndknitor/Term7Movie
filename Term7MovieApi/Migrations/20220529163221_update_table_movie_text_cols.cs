using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Term7MovieApi.Migrations
{
    public partial class update_table_movie_text_cols : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TrailerUrl",
                table: "Movies",
                type: "varchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Movies",
                type: "nvarchar(300)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AlterColumn<string>(
                name: "PosterImageUrl",
                table: "Movies",
                type: "varchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CoverImageUrl",
                table: "Movies",
                type: "varchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TrailerUrl",
                table: "Movies",
                type: "varchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Movies",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)");

            migrationBuilder.AlterColumn<string>(
                name: "PosterImageUrl",
                table: "Movies",
                type: "varchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Movies",
                type: "nvarchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CoverImageUrl",
                table: "Movies",
                type: "varchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldNullable: true);
        }
    }
}
