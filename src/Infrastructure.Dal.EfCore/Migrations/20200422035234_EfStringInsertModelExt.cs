using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Dal.EfCore.Migrations
{
    public partial class EfStringInsertModelExt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StringInseartModelExt",
                columns: table => new
                {
                    Key = table.Column<string>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    Format = table.Column<string>(maxLength: 50, nullable: true),
                    BorderSubString = table.Column<string>(nullable: true),
                    StringMiddleWareOption = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StringInseartModelExt", x => x.Key);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StringInseartModelExt");
        }
    }
}
