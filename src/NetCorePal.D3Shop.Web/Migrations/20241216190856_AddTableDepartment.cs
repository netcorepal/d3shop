using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetCorePal.D3Shop.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddTableDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_adminUserPermissions_adminUsers_AdminUserId",
                table: "adminUserPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_adminUserRoles_adminUsers_AdminUserId",
                table: "adminUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_adminUserRoles_roles_RoleId",
                table: "adminUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_adminUserRoles_RoleId",
                table: "adminUserRoles");

            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departments", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "userDepts",
                columns: table => new
                {
                    AdminUserId = table.Column<long>(type: "bigint", nullable: false),
                    DeptId = table.Column<long>(type: "bigint", nullable: false),
                    DeptName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userDepts", x => new { x.AdminUserId, x.DeptId });
                    table.ForeignKey(
                        name: "FK_userDepts_adminUsers_AdminUserId",
                        column: x => x.AdminUserId,
                        principalTable: "adminUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_userDepts_AdminUserId",
                table: "userDepts",
                column: "AdminUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_adminUserPermissions_adminUsers_AdminUserId",
                table: "adminUserPermissions",
                column: "AdminUserId",
                principalTable: "adminUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_adminUserRoles_adminUsers_AdminUserId",
                table: "adminUserRoles",
                column: "AdminUserId",
                principalTable: "adminUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_adminUserPermissions_adminUsers_AdminUserId",
                table: "adminUserPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_adminUserRoles_adminUsers_AdminUserId",
                table: "adminUserRoles");

            migrationBuilder.DropTable(
                name: "departments");

            migrationBuilder.DropTable(
                name: "userDepts");

            migrationBuilder.CreateIndex(
                name: "IX_adminUserRoles_RoleId",
                table: "adminUserRoles",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_adminUserPermissions_adminUsers_AdminUserId",
                table: "adminUserPermissions",
                column: "AdminUserId",
                principalTable: "adminUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_adminUserRoles_adminUsers_AdminUserId",
                table: "adminUserRoles",
                column: "AdminUserId",
                principalTable: "adminUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_adminUserRoles_roles_RoleId",
                table: "adminUserRoles",
                column: "RoleId",
                principalTable: "roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
