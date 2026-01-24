using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickleballClubManagement.Migrations
{
    /// <inheritdoc />
    public partial class RenameTablesTo186 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "089_Bookings",
                newName: "186_Bookings");

            migrationBuilder.RenameTable(
                name: "089_Challenges",
                newName: "186_Challenges");

            migrationBuilder.RenameTable(
                name: "089_Courts",
                newName: "186_Courts");

            migrationBuilder.RenameTable(
                name: "089_Matches",
                newName: "186_Matches");

            migrationBuilder.RenameTable(
                name: "089_Members",
                newName: "186_Members");

            migrationBuilder.RenameTable(
                name: "089_News",
                newName: "186_News");

            migrationBuilder.RenameTable(
                name: "089_Participants",
                newName: "186_Participants");

            migrationBuilder.RenameTable(
                name: "089_Transactions",
                newName: "186_Transactions");

            migrationBuilder.RenameTable(
                name: "089_TransactionCategories",
                newName: "186_TransactionCategories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "186_Bookings",
                newName: "089_Bookings");

            migrationBuilder.RenameTable(
                name: "186_Challenges",
                newName: "089_Challenges");

            migrationBuilder.RenameTable(
                name: "186_Courts",
                newName: "089_Courts");

            migrationBuilder.RenameTable(
                name: "186_Matches",
                newName: "089_Matches");

            migrationBuilder.RenameTable(
                name: "186_Members",
                newName: "089_Members");

            migrationBuilder.RenameTable(
                name: "186_News",
                newName: "089_News");

            migrationBuilder.RenameTable(
                name: "186_Participants",
                newName: "089_Participants");

            migrationBuilder.RenameTable(
                name: "186_Transactions",
                newName: "089_Transactions");

            migrationBuilder.RenameTable(
                name: "186_TransactionCategories",
                newName: "089_TransactionCategories");
        }
    }
}
