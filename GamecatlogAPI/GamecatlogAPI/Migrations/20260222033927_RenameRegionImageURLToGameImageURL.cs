using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamecatalogAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenameRegionImageURLToGameImageURL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RegionImageURL",
                table: "Game",
                newName: "GameImageURL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GameImageURL",
                table: "Game",
                newName: "RegionImageURL");
        }
    }
}
