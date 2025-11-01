using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Play.OrderService.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderTotalcolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "OrderTotal",
                table: "UserOrders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderTotal",
                table: "UserOrders");
        }
    }
}
