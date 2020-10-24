using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CvCreator.Api.Migrations
{
    public partial class filledTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FilledTemplate",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TemplateId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilledTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FilledElement",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FilledText = table.Column<string>(nullable: true),
                    FilledTemplateId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilledElement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FilledElement_FilledTemplate_FilledTemplateId",
                        column: x => x.FilledTemplateId,
                        principalTable: "FilledTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilledElement_FilledTemplateId",
                table: "FilledElement",
                column: "FilledTemplateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilledElement");

            migrationBuilder.DropTable(
                name: "FilledTemplate");
        }
    }
}
