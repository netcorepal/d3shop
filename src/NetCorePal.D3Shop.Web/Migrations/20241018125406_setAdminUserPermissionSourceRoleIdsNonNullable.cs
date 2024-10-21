using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetCorePal.D3Shop.Web.Migrations
{
    /// <inheritdoc />
    public partial class setAdminUserPermissionSourceRoleIdsNonNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "adminUserPermissions",
                keyColumn: "SourceRoleIds",
                keyValue: null,
                column: "SourceRoleIds",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "SourceRoleIds",
                table: "adminUserPermissions",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SourceRoleIds",
                table: "adminUserPermissions",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
