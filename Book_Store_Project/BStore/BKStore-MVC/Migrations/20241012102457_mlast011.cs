using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BKStore_MVC.Migrations
{
    /// <inheritdoc />
    public partial class mlast011 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_shippingMethod_GovernorateID",
                table: "shippingMethod",
                column: "GovernorateID");

            migrationBuilder.AddForeignKey(
                name: "FK_shippingMethod_governorate_GovernorateID",
                table: "shippingMethod",
                column: "GovernorateID",
                principalTable: "governorate",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_shippingMethod_governorate_GovernorateID",
                table: "shippingMethod");

            migrationBuilder.DropIndex(
                name: "IX_shippingMethod_GovernorateID",
                table: "shippingMethod");
        }
    }
}
