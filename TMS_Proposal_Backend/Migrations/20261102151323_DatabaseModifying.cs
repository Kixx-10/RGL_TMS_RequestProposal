using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS_Proposal_Backend.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseModifying : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverLicenseNo",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "DriverName",
                table: "Requests");

            migrationBuilder.RenameColumn(
                name: "RegistrationDateTime",
                table: "Requests",
                newName: "ETA");

            migrationBuilder.AddColumn<string>(
                name: "CompanyAddress",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomerPhoneNo",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Requests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileUrl",
                table: "Requests",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Trucks",
                columns: table => new
                {
                    TruckId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TruckNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trucks", x => x.TruckId);
                    table.ForeignKey(
                        name: "FK_Trucks_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trucks_RequestId",
                table: "Trucks",
                column: "RequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trucks");

            migrationBuilder.DropColumn(
                name: "CompanyAddress",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CustomerPhoneNo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "FileUrl",
                table: "Requests");

            migrationBuilder.RenameColumn(
                name: "ETA",
                table: "Requests",
                newName: "RegistrationDateTime");

            migrationBuilder.AddColumn<string>(
                name: "DriverLicenseNo",
                table: "Requests",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DriverName",
                table: "Requests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
