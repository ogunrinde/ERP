using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ICSERP.Migrations
{
    public partial class updateUserManagement5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpecialPermissions",
                columns: table => new
                {
                    SpecialPermissionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    PermissionId = table.Column<int>(nullable: false),
                    DocumentType = table.Column<string>(nullable: true),
                    DocumentAccessLevel = table.Column<int>(nullable: false),
                    Create = table.Column<bool>(nullable: false),
                    Update = table.Column<bool>(nullable: false),
                    Read = table.Column<bool>(nullable: false),
                    Delete = table.Column<bool>(nullable: false),
                    Upload = table.Column<bool>(nullable: false),
                    Download = table.Column<bool>(nullable: false),
                    Approval = table.Column<bool>(nullable: false),
                    Amend = table.Column<bool>(nullable: false),
                    Cancel = table.Column<bool>(nullable: false),
                    SetPermission = table.Column<bool>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialPermissions", x => x.SpecialPermissionId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpecialPermissions");
        }
    }
}
