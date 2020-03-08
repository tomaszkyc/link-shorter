using Microsoft.EntityFrameworkCore.Migrations;

namespace LinkShorter.Migrations
{
    public partial class changedEntityName2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ads_AspNetUsers_AdOwnerId",
                table: "Ads");

            migrationBuilder.DropForeignKey(
                name: "FK_UrlStatistics_Ads_AdId",
                table: "UrlStatistics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ads",
                table: "Ads");

            migrationBuilder.RenameTable(
                name: "Ads",
                newName: "Links");

            migrationBuilder.RenameIndex(
                name: "IX_Ads_AdOwnerId",
                table: "Links",
                newName: "IX_Links_AdOwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Links",
                table: "Links",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Links_AspNetUsers_AdOwnerId",
                table: "Links",
                column: "AdOwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UrlStatistics_Links_AdId",
                table: "UrlStatistics",
                column: "AdId",
                principalTable: "Links",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Links_AspNetUsers_AdOwnerId",
                table: "Links");

            migrationBuilder.DropForeignKey(
                name: "FK_UrlStatistics_Links_AdId",
                table: "UrlStatistics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Links",
                table: "Links");

            migrationBuilder.RenameTable(
                name: "Links",
                newName: "Ads");

            migrationBuilder.RenameIndex(
                name: "IX_Links_AdOwnerId",
                table: "Ads",
                newName: "IX_Ads_AdOwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ads",
                table: "Ads",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ads_AspNetUsers_AdOwnerId",
                table: "Ads",
                column: "AdOwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UrlStatistics_Ads_AdId",
                table: "UrlStatistics",
                column: "AdId",
                principalTable: "Ads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
