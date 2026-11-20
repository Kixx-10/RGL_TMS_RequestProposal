using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS_Proposal_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddIsArrivedToTruck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsArrived",
                table: "Trucks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArrived",
                table: "Trucks");
        }
    }
}
