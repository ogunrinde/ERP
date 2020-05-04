using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ICSERP.Migrations
{
    public partial class updateUserManagement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeactivated",
                table: "Units",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Is_deleted",
                table: "Units",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeactivated",
                table: "Roles",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeactivated",
                table: "Permissions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Is_deleted",
                table: "Permissions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeactivated",
                table: "Level",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Is_deleted",
                table: "Level",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeactivated",
                table: "Department",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Is_deleted",
                table: "Department",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeactivated",
                table: "Companies",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Is_deleted",
                table: "Companies",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeactivated",
                table: "Branches",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Is_deleted",
                table: "Branches",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateDeactivated",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "Is_deleted",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "DateDeactivated",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "DateDeactivated",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "Is_deleted",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "DateDeactivated",
                table: "Level");

            migrationBuilder.DropColumn(
                name: "Is_deleted",
                table: "Level");

            migrationBuilder.DropColumn(
                name: "DateDeactivated",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "Is_deleted",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "DateDeactivated",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Is_deleted",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "DateDeactivated",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "Is_deleted",
                table: "Branches");
        }
    }
}
