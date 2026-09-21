using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BhumiLogistics.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLandUseAndSetbackToLandPlots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "FrontageLengthInMeters",
                table: "LandPlots",
                type: "numeric(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "LandUseClassification",
                table: "LandPlots",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "LandUseConversionApprovalDate",
                table: "LandPlots",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LandUseConversionApprovalReferenceNumber",
                table: "LandPlots",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LandUseConversionApprovingAuthority",
                table: "LandPlots",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SetbackDistanceInMeters",
                table: "LandPlots",
                type: "numeric(6,2)",
                precision: 6,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "HighwaySetbackStandards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HighwayFrontageType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SetbackDistanceInMeters = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HighwaySetbackStandards", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "HighwaySetbackStandards",
                columns: new[] { "Id", "HighwayFrontageType", "Notes", "SetbackDistanceInMeters" },
                values: new object[,]
                {
                    { new Guid("11111111-0000-0000-0000-000000000001"), "None", null, 0m },
                    { new Guid("11111111-0000-0000-0000-000000000002"), "StateHighwayFrontage", "Approximate — verify against current Department of Roads standard.", 15m },
                    { new Guid("11111111-0000-0000-0000-000000000003"), "NationalHighwayFrontage", "Approximate — verify against current Department of Roads standard.", 25m },
                    { new Guid("11111111-0000-0000-0000-000000000004"), "ExpresswayFrontage", "Approximate — draft 2083 bill proposes 50m per side plus an additional 6m no-build buffer.", 50m },
                    { new Guid("11111111-0000-0000-0000-000000000005"), "CornerPlotDualFrontage", "Conservative default for dual frontage — confirm which of the two roads governs.", 25m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_HighwaySetbackStandards_HighwayFrontageType",
                table: "HighwaySetbackStandards",
                column: "HighwayFrontageType",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HighwaySetbackStandards");

            migrationBuilder.DropColumn(
                name: "FrontageLengthInMeters",
                table: "LandPlots");

            migrationBuilder.DropColumn(
                name: "LandUseClassification",
                table: "LandPlots");

            migrationBuilder.DropColumn(
                name: "LandUseConversionApprovalDate",
                table: "LandPlots");

            migrationBuilder.DropColumn(
                name: "LandUseConversionApprovalReferenceNumber",
                table: "LandPlots");

            migrationBuilder.DropColumn(
                name: "LandUseConversionApprovingAuthority",
                table: "LandPlots");

            migrationBuilder.DropColumn(
                name: "SetbackDistanceInMeters",
                table: "LandPlots");
        }
    }
}
