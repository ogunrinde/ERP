using Microsoft.EntityFrameworkCore.Migrations;

namespace ICSERP.Migrations
{
    public partial class updateUserManagement7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Is_deleted",
                table: "SpecialPermissions",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Is_deleted",
                table: "SpecialPermissions",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool));
        }
    }
}
