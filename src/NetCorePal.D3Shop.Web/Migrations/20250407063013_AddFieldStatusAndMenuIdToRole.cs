using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetCorePal.D3Shop.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldStatusAndMenuIdToRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeptCode",
                table: "userDepts",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "userDepts",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "roles",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "roles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "MenuId",
                table: "rolePermissions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "adminUsers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "RealName",
                table: "adminUsers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeptCode",
                table: "userDepts");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "userDepts");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "rolePermissions");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "adminUsers");

            migrationBuilder.DropColumn(
                name: "RealName",
                table: "adminUsers");
        }
    }
}
