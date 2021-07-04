using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace TTRBookings.Infrastructure.Data.Migrations
{
    public partial class RoseTiers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roses_Houses_HouseId",
                table: "Roses");

            migrationBuilder.DropForeignKey(
                name: "FK_Tiers_Roses_RoseId",
                table: "Tiers");

            migrationBuilder.DropIndex(
                name: "IX_Tiers_RoseId",
                table: "Tiers");

            migrationBuilder.DropColumn(
                name: "RoseId",
                table: "Tiers");

            migrationBuilder.AlterColumn<Guid>(
                name: "HouseId",
                table: "Roses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Roses_Houses_HouseId",
                table: "Roses",
                column: "HouseId",
                principalTable: "Houses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roses_Houses_HouseId",
                table: "Roses");

            migrationBuilder.AddColumn<Guid>(
                name: "RoseId",
                table: "Tiers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "HouseId",
                table: "Roses",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Tiers_RoseId",
                table: "Tiers",
                column: "RoseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roses_Houses_HouseId",
                table: "Roses",
                column: "HouseId",
                principalTable: "Houses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tiers_Roses_RoseId",
                table: "Tiers",
                column: "RoseId",
                principalTable: "Roses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
