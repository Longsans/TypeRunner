using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TypeRunnerBE.Migrations
{
    /// <inheritdoc />
    public partial class RenameAuthorAndQuoteThenAddMistakeNumOfTimes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Races_Quotes_QuoteId",
                table: "Races");

            migrationBuilder.DropTable(
                name: "Quotes");

            migrationBuilder.DropTable(
                name: "UserRaceMistake");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.RenameColumn(
                name: "QuoteId",
                table: "Races",
                newName: "PassageId");

            migrationBuilder.RenameIndex(
                name: "IX_Races_QuoteId",
                table: "Races",
                newName: "IX_Races_PassageId");

            migrationBuilder.AddColumn<int>(
                name: "AverageWpm",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "Races",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRaceMistakes",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    RaceId = table.Column<int>(type: "integer", nullable: false),
                    Word = table.Column<string>(type: "text", nullable: false),
                    NumberOfTimes = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRaceMistakes", x => new { x.UserId, x.RaceId, x.Word });
                    table.ForeignKey(
                        name: "FK_UserRaceMistakes_UserRaceRecords_UserId_RaceId",
                        columns: x => new { x.UserId, x.RaceId },
                        principalTable: "UserRaceRecords",
                        principalColumns: new[] { "UserId", "RaceId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Passages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "text", nullable: false),
                    SourceId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Passages_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Passages_SourceId",
                table: "Passages",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_Name",
                table: "Sources",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Races_Passages_PassageId",
                table: "Races",
                column: "PassageId",
                principalTable: "Passages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Races_Passages_PassageId",
                table: "Races");

            migrationBuilder.DropTable(
                name: "Passages");

            migrationBuilder.DropTable(
                name: "UserRaceMistakes");

            migrationBuilder.DropTable(
                name: "Sources");

            migrationBuilder.DropColumn(
                name: "AverageWpm",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "PassageId",
                table: "Races",
                newName: "QuoteId");

            migrationBuilder.RenameIndex(
                name: "IX_Races_PassageId",
                table: "Races",
                newName: "IX_Races_QuoteId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "Races",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRaceMistake",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    RaceId = table.Column<int>(type: "integer", nullable: false),
                    Word = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRaceMistake", x => new { x.UserId, x.RaceId, x.Word });
                    table.ForeignKey(
                        name: "FK_UserRaceMistake_UserRaceRecords_UserId_RaceId",
                        columns: x => new { x.UserId, x.RaceId },
                        principalTable: "UserRaceRecords",
                        principalColumns: new[] { "UserId", "RaceId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Quotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuthorId = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quotes_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_AuthorId",
                table: "Quotes",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Races_Quotes_QuoteId",
                table: "Races",
                column: "QuoteId",
                principalTable: "Quotes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
