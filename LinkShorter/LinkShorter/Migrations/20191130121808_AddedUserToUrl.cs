using Microsoft.EntityFrameworkCore.Migrations;

namespace LinkShorter.Migrations
{
    public partial class AddedUserToUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Ads",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ads_IdentityUserId",
                table: "Ads",
                column: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ads_AspNetUsers_IdentityUserId",
                table: "Ads",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ads_AspNetUsers_IdentityUserId",
                table: "Ads");

            migrationBuilder.DropIndex(
                name: "IX_Ads_IdentityUserId",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Ads");
        }
    }
}
