using Microsoft.EntityFrameworkCore.Migrations;

namespace LinkShorter.Migrations
{
    public partial class ChangedEntityName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UrlStatistics_Links_AdId",
                table: "UrlStatistics");

            migrationBuilder.RenameColumn(
                name: "AdId",
                table: "UrlStatistics",
                newName: "LinkId");

            migrationBuilder.RenameIndex(
                name: "IX_UrlStatistics_AdId",
                table: "UrlStatistics",
                newName: "IX_UrlStatistics_LinkId");

            migrationBuilder.AddForeignKey(
                name: "FK_UrlStatistics_Links_LinkId",
                table: "UrlStatistics",
                column: "LinkId",
                principalTable: "Links",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UrlStatistics_Links_LinkId",
                table: "UrlStatistics");

            migrationBuilder.RenameColumn(
                name: "LinkId",
                table: "UrlStatistics",
                newName: "AdId");

            migrationBuilder.RenameIndex(
                name: "IX_UrlStatistics_LinkId",
                table: "UrlStatistics",
                newName: "IX_UrlStatistics_AdId");

            migrationBuilder.AddForeignKey(
                name: "FK_UrlStatistics_Links_AdId",
                table: "UrlStatistics",
                column: "AdId",
                principalTable: "Links",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
