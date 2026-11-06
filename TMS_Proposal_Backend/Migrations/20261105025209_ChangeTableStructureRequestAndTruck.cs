using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS_Proposal_Backend.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTableStructureRequestAndTruck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Users_CreatedByUserId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ETA",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "JobType",
                table: "Requests");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDated",
                table: "Trucks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ETA",
                table: "Trucks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "JobType",
                table: "Trucks",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Trucks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Users_CreatedByUserId",
                table: "Requests",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Users_CreatedByUserId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "CreatedDated",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "ETA",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "JobType",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Trucks");

            migrationBuilder.AddColumn<DateTime>(
                name: "ETA",
                table: "Requests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "JobType",
                table: "Requests",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Users_CreatedByUserId",
                table: "Requests",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
