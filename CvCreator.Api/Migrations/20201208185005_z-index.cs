using Microsoft.EntityFrameworkCore.Migrations;

namespace CvCreator.Api.Migrations
{
    public partial class zindex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ZIndex",
                table: "Content",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZIndex",
                table: "Content");
        }
    }
}
