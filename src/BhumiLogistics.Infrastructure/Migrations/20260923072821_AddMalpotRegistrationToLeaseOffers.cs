using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BhumiLogistics.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMalpotRegistrationToLeaseOffers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MalpotRegistrationStatus",
                table: "LeaseOffers",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RegisteredAtUtc",
                table: "LeaseOffers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RegisteredByUserId",
                table: "LeaseOffers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegisteredDeedReferenceNumber",
                table: "LeaseOffers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "RegistrationDate",
                table: "LeaseOffers",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MalpotRegistrationStatus",
                table: "LeaseOffers");

            migrationBuilder.DropColumn(
                name: "RegisteredAtUtc",
                table: "LeaseOffers");

            migrationBuilder.DropColumn(
                name: "RegisteredByUserId",
                table: "LeaseOffers");

            migrationBuilder.DropColumn(
                name: "RegisteredDeedReferenceNumber",
                table: "LeaseOffers");

            migrationBuilder.DropColumn(
                name: "RegistrationDate",
                table: "LeaseOffers");
        }
    }
}
