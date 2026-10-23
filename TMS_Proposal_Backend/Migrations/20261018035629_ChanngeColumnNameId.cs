using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS_Proposal_Backend.Migrations
{
    /// <inheritdoc />
    public partial class ChanngeColumnNameId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "Requests",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Requests",
                newName: "id");
        }
    }
}
