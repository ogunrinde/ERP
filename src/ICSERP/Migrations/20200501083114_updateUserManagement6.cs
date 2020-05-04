using Microsoft.EntityFrameworkCore.Migrations;

namespace ICSERP.Migrations
{
    public partial class updateUserManagement6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PermissionId",
                table: "SpecialPermissions");

            migrationBuilder.AddColumn<int>(
                name: "Is_deleted",
                table: "SpecialPermissions",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Is_deleted",
                table: "SpecialPermissions");

            migrationBuilder.AddColumn<int>(
                name: "PermissionId",
                table: "SpecialPermissions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
