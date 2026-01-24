using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickleballClubManagement.Migrations
{
    /// <inheritdoc />
    public partial class MakeCourtIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_089_Bookings_089_Courts_CourtId",
                table: "089_Bookings");

            migrationBuilder.AlterColumn<int>(
                name: "CourtId",
                table: "089_Bookings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_089_Bookings_089_Courts_CourtId",
                table: "089_Bookings",
                column: "CourtId",
                principalTable: "089_Courts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_089_Bookings_089_Courts_CourtId",
                table: "089_Bookings");

            migrationBuilder.AlterColumn<int>(
                name: "CourtId",
                table: "089_Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_089_Bookings_089_Courts_CourtId",
                table: "089_Bookings",
                column: "CourtId",
                principalTable: "089_Courts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
