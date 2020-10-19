using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Dal.EfCore.Migrations
{
    public partial class Add_InlineStringInsertModels_Entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InlineStringInsertModels",
                columns: table => new
                {
                    Key = table.Column<string>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    InlineStr = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InlineStringInsertModels", x => x.Key);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InlineStringInsertModels");
        }
    }
}
