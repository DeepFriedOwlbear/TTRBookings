using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TTRBookings.Migrations
{
    public partial class MigratedBookings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Roses_RoseId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_TimeSlots_TimeSlotId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeSlots_Houses_HouseId",
                table: "TimeSlots");

            migrationBuilder.DropIndex(
                name: "IX_TimeSlots_HouseId",
                table: "TimeSlots");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_RoseId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_TimeSlotId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "HouseId",
                table: "TimeSlots");

            migrationBuilder.DropColumn(
                name: "RoseId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "TimeSlotId",
                table: "Rooms");

            migrationBuilder.AddColumn<DateTime>(
                name: "End",
                table: "TimeSlots",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TimeSlotId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Houses_HouseId",
                        column: x => x.HouseId,
                        principalTable: "Houses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Roses_RoseId",
                        column: x => x.RoseId,
                        principalTable: "Roses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Tiers_TierId",
                        column: x => x.TierId,
                        principalTable: "Tiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_TimeSlots_TimeSlotId",
                        column: x => x.TimeSlotId,
                        principalTable: "TimeSlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_HouseId",
                table: "Bookings",
                column: "HouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RoomId",
                table: "Bookings",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RoseId",
                table: "Bookings",
                column: "RoseId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TierId",
                table: "Bookings",
                column: "TierId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TimeSlotId",
                table: "Bookings",
                column: "TimeSlotId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropColumn(
                name: "End",
                table: "TimeSlots");

            migrationBuilder.AddColumn<Guid>(
                name: "HouseId",
                table: "TimeSlots",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RoseId",
                table: "Rooms",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TimeSlotId",
                table: "Rooms",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimeSlots_HouseId",
                table: "TimeSlots",
                column: "HouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoseId",
                table: "Rooms",
                column: "RoseId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_TimeSlotId",
                table: "Rooms",
                column: "TimeSlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Roses_RoseId",
                table: "Rooms",
                column: "RoseId",
                principalTable: "Roses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_TimeSlots_TimeSlotId",
                table: "Rooms",
                column: "TimeSlotId",
                principalTable: "TimeSlots",
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
    }
}
