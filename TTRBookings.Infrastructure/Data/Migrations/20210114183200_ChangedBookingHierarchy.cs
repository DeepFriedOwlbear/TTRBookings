using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TTRBookings.Infrastructure.Data.Migrations
{
    public partial class ChangedBookingHierarchy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TierLists",
                table: "TierLists");

            migrationBuilder.RenameTable(
                name: "TierLists",
                newName: "TierRates");

            migrationBuilder.AddColumn<Guid>(
                name: "HouseId",
                table: "TimeSlots",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TimeSlotId",
                table: "Rooms",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "HouseId",
                table: "TierRates",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TierRates",
                table: "TierRates",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TimeSlots_HouseId",
                table: "TimeSlots",
                column: "HouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_TimeSlotId",
                table: "Rooms",
                column: "TimeSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_TierRates_HouseId",
                table: "TierRates",
                column: "HouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_TimeSlots_TimeSlotId",
                table: "Rooms",
                column: "TimeSlotId",
                principalTable: "TimeSlots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TierRates_Houses_HouseId",
                table: "TierRates",
                column: "HouseId",
                principalTable: "Houses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSlots_Houses_HouseId",
                table: "TimeSlots",
                column: "HouseId",
                principalTable: "Houses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_TimeSlots_TimeSlotId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_TierRates_Houses_HouseId",
                table: "TierRates");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeSlots_Houses_HouseId",
                table: "TimeSlots");

            migrationBuilder.DropIndex(
                name: "IX_TimeSlots_HouseId",
                table: "TimeSlots");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_TimeSlotId",
                table: "Rooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TierRates",
                table: "TierRates");

            migrationBuilder.DropIndex(
                name: "IX_TierRates_HouseId",
                table: "TierRates");

            migrationBuilder.DropColumn(
                name: "HouseId",
                table: "TimeSlots");

            migrationBuilder.DropColumn(
                name: "TimeSlotId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "HouseId",
                table: "TierRates");

            migrationBuilder.RenameTable(
                name: "TierRates",
                newName: "TierLists");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TierLists",
                table: "TierLists",
                column: "Id");
        }
    }
}
