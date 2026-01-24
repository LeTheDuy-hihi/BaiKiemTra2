using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickleballClubManagement.Migrations
{
    /// <inheritdoc />
    public partial class AdvancedUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "089_Members",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "089_Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "089_Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalMatches",
                table: "089_Members",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WinMatches",
                table: "089_Members",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WinningSide",
                table: "089_Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Config_TargetWins",
                table: "089_Challenges",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentScore_TeamA",
                table: "089_Challenges",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CurrentScore_TeamB",
                table: "089_Challenges",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "089_Challenges",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "EntryFee",
                table: "089_Challenges",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "089_Challenges",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PrizePool",
                table: "089_Challenges",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ResultMode",
                table: "089_Challenges",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "089_Challenges",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "089_Challenges",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CourtId",
                table: "089_Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "089_Bookings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "089_Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "089_Courts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_089_Courts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "089_News",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPinned = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_089_News", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "089_Participants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChallengeId = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    Team = table.Column<int>(type: "int", nullable: false),
                    EntryFeePaid = table.Column<bool>(type: "bit", nullable: false),
                    EntryFeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    JoinedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_089_Participants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_089_Participants_089_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "089_Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_089_Participants_089_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "089_Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "089_TransactionCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_089_TransactionCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "089_Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_089_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_089_Transactions_089_TransactionCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "089_TransactionCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_089_Bookings_CourtId",
                table: "089_Bookings",
                column: "CourtId");

            migrationBuilder.CreateIndex(
                name: "IX_089_Participants_ChallengeId",
                table: "089_Participants",
                column: "ChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_089_Participants_MemberId",
                table: "089_Participants",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_089_Transactions_CategoryId",
                table: "089_Transactions",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_089_Bookings_089_Courts_CourtId",
                table: "089_Bookings",
                column: "CourtId",
                principalTable: "089_Courts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_089_Bookings_089_Courts_CourtId",
                table: "089_Bookings");

            migrationBuilder.DropTable(
                name: "089_Courts");

            migrationBuilder.DropTable(
                name: "089_News");

            migrationBuilder.DropTable(
                name: "089_Participants");

            migrationBuilder.DropTable(
                name: "089_Transactions");

            migrationBuilder.DropTable(
                name: "089_TransactionCategories");

            migrationBuilder.DropIndex(
                name: "IX_089_Bookings_CourtId",
                table: "089_Bookings");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "089_Members");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "089_Members");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "089_Members");

            migrationBuilder.DropColumn(
                name: "TotalMatches",
                table: "089_Members");

            migrationBuilder.DropColumn(
                name: "WinMatches",
                table: "089_Members");

            migrationBuilder.DropColumn(
                name: "WinningSide",
                table: "089_Matches");

            migrationBuilder.DropColumn(
                name: "Config_TargetWins",
                table: "089_Challenges");

            migrationBuilder.DropColumn(
                name: "CurrentScore_TeamA",
                table: "089_Challenges");

            migrationBuilder.DropColumn(
                name: "CurrentScore_TeamB",
                table: "089_Challenges");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "089_Challenges");

            migrationBuilder.DropColumn(
                name: "EntryFee",
                table: "089_Challenges");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "089_Challenges");

            migrationBuilder.DropColumn(
                name: "PrizePool",
                table: "089_Challenges");

            migrationBuilder.DropColumn(
                name: "ResultMode",
                table: "089_Challenges");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "089_Challenges");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "089_Challenges");

            migrationBuilder.DropColumn(
                name: "CourtId",
                table: "089_Bookings");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "089_Bookings");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "089_Bookings");
        }
    }
}
