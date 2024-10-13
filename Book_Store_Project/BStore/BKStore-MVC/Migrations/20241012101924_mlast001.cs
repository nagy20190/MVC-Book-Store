using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BKStore_MVC.Migrations
{
    /// <inheritdoc />
    public partial class mlast001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingMethod",
                table: "Shipping");

            migrationBuilder.AddColumn<int>(
                name: "ShippingMethodID",
                table: "Shipping",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "shippingMethod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GovernorateID = table.Column<int>(type: "int", nullable: true),
                    PaymentFees = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shippingMethod", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shipping_ShippingMethodID",
                table: "Shipping",
                column: "ShippingMethodID");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipping_shippingMethod_ShippingMethodID",
                table: "Shipping",
                column: "ShippingMethodID",
                principalTable: "shippingMethod",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipping_shippingMethod_ShippingMethodID",
                table: "Shipping");

            migrationBuilder.DropTable(
                name: "shippingMethod");

            migrationBuilder.DropIndex(
                name: "IX_Shipping_ShippingMethodID",
                table: "Shipping");

            migrationBuilder.DropColumn(
                name: "ShippingMethodID",
                table: "Shipping");

            migrationBuilder.AddColumn<string>(
                name: "ShippingMethod",
                table: "Shipping",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);
        }
    }
}
