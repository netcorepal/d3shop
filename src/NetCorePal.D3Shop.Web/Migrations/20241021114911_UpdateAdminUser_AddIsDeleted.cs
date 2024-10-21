using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetCorePal.D3Shop.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminUser_AddIsDeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RefreshTokenExpiryDate",
                table: "adminUsers",
                newName: "LoginExpiryDate");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "adminUsers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "adminUsers");

            migrationBuilder.RenameColumn(
                name: "LoginExpiryDate",
                table: "adminUsers",
                newName: "RefreshTokenExpiryDate");
        }
    }
}
