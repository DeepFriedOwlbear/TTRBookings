using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TTRBookings.Infrastructure.Data.Migrations
{
    public partial class RenameToStaff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Roses_RoseId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Tiers_Roses_RoseId",
                table: "Tiers");

            migrationBuilder.DropTable(
                name: "Roses");

            migrationBuilder.RenameColumn(
                name: "RoseId",
                table: "Tiers",
                newName: "StaffId");

            migrationBuilder.RenameIndex(
                name: "IX_Tiers_RoseId",
                table: "Tiers",
                newName: "IX_Tiers_StaffId");

            migrationBuilder.RenameColumn(
                name: "RoseCut",
                table: "Houses",
                newName: "StaffCut");

            migrationBuilder.RenameColumn(
                name: "RoseId",
                table: "Bookings",
                newName: "StaffId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_RoseId",
                table: "Bookings",
                newName: "IX_Bookings_StaffId");

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Staff_Houses_HouseId",
                        column: x => x.HouseId,
                        principalTable: "Houses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Staff_HouseId",
                table: "Staff",
                column: "HouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Staff_StaffId",
                table: "Bookings",
                column: "StaffId",
                principalTable: "Staff",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tiers_Staff_StaffId",
                table: "Tiers",
                column: "StaffId",
                principalTable: "Staff",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Staff_StaffId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Tiers_Staff_StaffId",
                table: "Tiers");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.RenameColumn(
                name: "StaffId",
                table: "Tiers",
                newName: "RoseId");

            migrationBuilder.RenameIndex(
                name: "IX_Tiers_StaffId",
                table: "Tiers",
                newName: "IX_Tiers_RoseId");

            migrationBuilder.RenameColumn(
                name: "StaffCut",
                table: "Houses",
                newName: "RoseCut");

            migrationBuilder.RenameColumn(
                name: "StaffId",
                table: "Bookings",
                newName: "RoseId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_StaffId",
                table: "Bookings",
                newName: "IX_Bookings_RoseId");

            migrationBuilder.CreateTable(
                name: "Roses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roses_Houses_HouseId",
                        column: x => x.HouseId,
                        principalTable: "Houses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Roses_HouseId",
                table: "Roses",
                column: "HouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Roses_RoseId",
                table: "Bookings",
                column: "RoseId",
                principalTable: "Roses",
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
