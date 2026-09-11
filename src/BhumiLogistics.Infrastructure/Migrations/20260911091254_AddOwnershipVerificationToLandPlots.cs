using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BhumiLogistics.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnershipVerificationToLandPlots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KittaNumber",
                table: "LandPlots",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LalpurjaReferenceNumber",
                table: "LandPlots",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LandIdentityNumber",
                table: "LandPlots",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MohiTenancyDeclared",
                table: "LandPlots",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MohiTenancyNotes",
                table: "LandPlots",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnershipStatus",
                table: "LandPlots",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OwnershipVerificationNotes",
                table: "LandPlots",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "OwnershipVerifiedAtUtc",
                table: "LandPlots",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OwnershipVerifiedByUserId",
                table: "LandPlots",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PowerOfAttorneyReferenceNumber",
                table: "LandPlots",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegisteredOwnerName",
                table: "LandPlots",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RelationshipToOwner",
                table: "LandPlots",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TenureType",
                table: "LandPlots",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WardMunicipality",
                table: "LandPlots",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KittaNumber",
                table: "LandPlots");

            migrationBuilder.DropColumn(
                name: "LalpurjaReferenceNumber",
                table: "LandPlots");

            migrationBuilder.DropColumn(
                name: "LandIdentityNumber",
                table: "LandPlots");

            migrationBuilder.DropColumn(
                name: "MohiTenancyDeclared",
                table: "LandPlots");

            migrationBuilder.DropColumn(
                name: "MohiTenancyNotes",
                table: "LandPlots");

            migrationBuilder.DropColumn(
                name: "OwnershipStatus",
                table: "LandPlots");

            migrationBuilder.DropColumn(
                name: "OwnershipVerificationNotes",
                table: "LandPlots");

            migrationBuilder.DropColumn(
                name: "OwnershipVerifiedAtUtc",
                table: "LandPlots");

            migrationBuilder.DropColumn(
                name: "OwnershipVerifiedByUserId",
                table: "LandPlots");

            migrationBuilder.DropColumn(
                name: "PowerOfAttorneyReferenceNumber",
                table: "LandPlots");

            migrationBuilder.DropColumn(
                name: "RegisteredOwnerName",
                table: "LandPlots");

            migrationBuilder.DropColumn(
                name: "RelationshipToOwner",
                table: "LandPlots");

            migrationBuilder.DropColumn(
                name: "TenureType",
                table: "LandPlots");

            migrationBuilder.DropColumn(
                name: "WardMunicipality",
                table: "LandPlots");
        }
    }
}
