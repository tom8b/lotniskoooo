using Microsoft.EntityFrameworkCore.Migrations;

namespace CvCreator.Api.Migrations
{
    public partial class profilepicture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsProfilePicture",
                table: "Element",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProfilePicture",
                table: "Element");
        }
    }
}
