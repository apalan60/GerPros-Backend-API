using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GerPros_Backend_API.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class allowSameSeriesName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BrandSeries_Name",
                table: "BrandSeries");

            migrationBuilder.CreateIndex(
                name: "IX_BrandSeries_Name",
                table: "BrandSeries",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BrandSeries_Name",
                table: "BrandSeries");

            migrationBuilder.CreateIndex(
                name: "IX_BrandSeries_Name",
                table: "BrandSeries",
                column: "Name",
                unique: true);
        }
    }
}
