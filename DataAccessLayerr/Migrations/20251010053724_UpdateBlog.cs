using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayerr.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BlogRating",
                table: "Blogss",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "Blogss",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlogRating",
                table: "Blogss");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Blogss");
        }
    }
}
