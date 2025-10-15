using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayerr.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBlogAppUserProfilPng : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilImage",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilImage",
                table: "AspNetUsers");
        }
    }
}
