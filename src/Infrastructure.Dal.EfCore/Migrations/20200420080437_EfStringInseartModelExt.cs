using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Dal.EfCore.Migrations
{
    public partial class EfStringInseartModelExt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StringInseartModelExt",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    VarName = table.Column<string>(maxLength: 256, nullable: false),
                    Format = table.Column<string>(maxLength: 50, nullable: true),
                    BorderSubString = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StringInseartModelExt", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StringInseartModelExt");
        }
    }
}
