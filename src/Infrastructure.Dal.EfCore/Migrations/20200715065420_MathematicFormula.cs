using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Dal.EfCore.Migrations
{
    public partial class MathematicFormula : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MathematicFormula",
                table: "StringInseartModelExt",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MathematicFormula",
                table: "StringInseartModelExt");
        }
    }
}
