using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Term7MovieApi.Migrations
{
    public partial class add_momo_payment_request_history : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentRequests",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartnerCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartnerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RedirectUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpnUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Items = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Signature = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentRequests", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentRequests");
        }
    }
}
