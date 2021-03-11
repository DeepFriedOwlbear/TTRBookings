using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TTRBookings.Infrastructure.Data.Migrations
{
    public partial class UpdatedMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RoseId",
                table: "Tiers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tiers_RoseId",
                table: "Tiers",
                column: "RoseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tiers_Roses_RoseId",
                table: "Tiers",
                column: "RoseId",
                principalTable: "Roses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tiers_Roses_RoseId",
                table: "Tiers");

            migrationBuilder.DropIndex(
                name: "IX_Tiers_RoseId",
                table: "Tiers");

            migrationBuilder.DropColumn(
                name: "RoseId",
                table: "Tiers");
        }
    }
}
