using Microsoft.EntityFrameworkCore.Migrations;

namespace CvCreator.Api.Migrations
{
    public partial class migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ElementKey",
                table: "Element",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ElementKey",
                table: "Element");
        }
    }
}
