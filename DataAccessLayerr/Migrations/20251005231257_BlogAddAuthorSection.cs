using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayerr.Migrations
{
    /// <inheritdoc />
    public partial class BlogAddAuthorSection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogss_Authors_AuthorID",
                table: "Blogss");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorID",
                table: "Blogss",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Blogss_Authors_AuthorID",
                table: "Blogss",
                column: "AuthorID",
                principalTable: "Authors",
                principalColumn: "AuthorID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogss_Authors_AuthorID",
                table: "Blogss");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorID",
                table: "Blogss",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogss_Authors_AuthorID",
                table: "Blogss",
                column: "AuthorID",
                principalTable: "Authors",
                principalColumn: "AuthorID");
        }
    }
}
