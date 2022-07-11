using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Term7MovieApi.Migrations
{
    public partial class adjust_range_theater_cols : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "Roles");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UserStatus",
                type: "nvarchar(30)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Users",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TransactionStatuses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "QRCodeUrl",
                table: "Transactions",
                type: "varchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200)");

            migrationBuilder.AlterColumn<string>(
                name: "TheaterName",
                table: "TransactionHistories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Theaters",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "Longitude",
                table: "Theaters",
                type: "varchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Latitude",
                table: "Theaters",
                type: "varchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Theaters",
                type: "nvarchar(200)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedName",
                table: "Roles",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Roles",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Movies",
                type: "nvarchar(300)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)");

            migrationBuilder.AlterColumn<string>(
                name: "LogoUrl",
                table: "Companies",
                type: "varchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200)");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "Roles");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UserStatus",
                type: "nvarchar(30)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Users",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TransactionStatuses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QRCodeUrl",
                table: "Transactions",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TheaterName",
                table: "TransactionHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Theaters",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AlterColumn<string>(
                name: "Longitude",
                table: "Theaters",
                type: "varchar(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Latitude",
                table: "Theaters",
                type: "varchar(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Theaters",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)");

            migrationBuilder.AlterColumn<string>(
                name: "NormalizedName",
                table: "Roles",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Roles",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Movies",
                type: "nvarchar(300)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LogoUrl",
                table: "Companies",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true);
        }
    }
}
