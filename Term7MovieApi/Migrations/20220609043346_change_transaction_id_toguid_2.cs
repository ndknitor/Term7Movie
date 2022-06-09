using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Term7MovieApi.Migrations
{
    public partial class change_transaction_id_toguid_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TempKey",
                table: "Transactions",
                newName: "Id");

            migrationBuilder.AddColumn<Guid>(
                name: "TransactionId",
                table: "TransactionHistories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TransactionId",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TransactionId",
                table: "Tickets",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Transactions_TransactionId",
                table: "Tickets",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Transactions_TransactionId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_TransactionId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "TransactionHistories");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Transactions",
                newName: "TempKey");
        }
    }
}
