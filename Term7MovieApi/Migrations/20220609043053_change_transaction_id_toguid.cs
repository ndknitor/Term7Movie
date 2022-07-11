using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Term7MovieApi.Migrations
{
    public partial class change_transaction_id_toguid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Transactions_TransactionId",
                table: "Tickets");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Transactions_TempKey",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_TransactionId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "TransactionHistories");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Tickets");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "TempKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "Transactions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<long>(
                name: "TransactionId",
                table: "TransactionHistories",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TransactionId",
                table: "Tickets",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Transactions_TempKey",
                table: "Transactions",
                column: "TempKey");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "Id");

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
    }
}
