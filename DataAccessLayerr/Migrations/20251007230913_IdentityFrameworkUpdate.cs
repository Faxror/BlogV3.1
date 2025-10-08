using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayerr.Migrations
{
    /// <inheritdoc />
    public partial class IdentityFrameworkUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "About",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "NameAndSunName",
                table: "AspNetUsers",
                newName: "Surname");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "AspNetUsers",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "AspNetUsers",
                newName: "NameAndSunName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AspNetUsers",
                newName: "Image");

            migrationBuilder.AddColumn<string>(
                name: "About",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
