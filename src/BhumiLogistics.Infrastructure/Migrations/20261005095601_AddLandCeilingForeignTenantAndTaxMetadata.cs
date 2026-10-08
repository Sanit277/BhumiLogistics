using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BhumiLogistics.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLandCeilingForeignTenantAndTaxMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyType",
                table: "Users",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DeclaredTotalLandHoldingInKattha",
                table: "Users",
                type: "numeric(12,2)",
                precision: 12,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartmentOfIndustryApprovalReferenceNumber",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FittaApprovalReferenceNumber",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ForeignInvestmentExtensionApproved",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OwnerType",
                table: "Users",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "LandCeilingReviewThresholdInKattha",
                table: "PlatformSettings",
                type: "numeric(12,2)",
                precision: 12,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "PlatformSettings",
                keyColumn: "Id",
                keyValue: new Guid("22222222-0000-0000-0000-000000000001"),
                column: "LandCeilingReviewThresholdInKattha",
                value: 100m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyType",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeclaredTotalLandHoldingInKattha",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DepartmentOfIndustryApprovalReferenceNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FittaApprovalReferenceNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ForeignInvestmentExtensionApproved",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OwnerType",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LandCeilingReviewThresholdInKattha",
                table: "PlatformSettings");
        }
    }
}
