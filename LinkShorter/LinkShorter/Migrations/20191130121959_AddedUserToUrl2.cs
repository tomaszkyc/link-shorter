using Microsoft.EntityFrameworkCore.Migrations;

namespace LinkShorter.Migrations
{
    public partial class AddedUserToUrl2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ads_AspNetUsers_IdentityUserId",
                table: "Ads");

            migrationBuilder.RenameColumn(
                name: "IdentityUserId",
                table: "Ads",
                newName: "AdOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Ads_IdentityUserId",
                table: "Ads",
                newName: "IX_Ads_AdOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ads_AspNetUsers_AdOwnerId",
                table: "Ads",
                column: "AdOwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ads_AspNetUsers_AdOwnerId",
                table: "Ads");

            migrationBuilder.RenameColumn(
                name: "AdOwnerId",
                table: "Ads",
                newName: "IdentityUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Ads_AdOwnerId",
                table: "Ads",
                newName: "IX_Ads_IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ads_AspNetUsers_IdentityUserId",
                table: "Ads",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
