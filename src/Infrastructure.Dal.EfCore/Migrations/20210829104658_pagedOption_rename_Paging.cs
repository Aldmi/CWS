using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Dal.EfCore.Migrations
{
    public partial class pagedOption_rename_Paging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PagedOption",
                table: "DeviceOptions");

            migrationBuilder.AddColumn<string>(
                name: "Paging",
                table: "DeviceOptions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Paging",
                table: "DeviceOptions");

            migrationBuilder.AddColumn<string>(
                name: "PagedOption",
                table: "DeviceOptions",
                type: "text",
                nullable: true);
        }
    }
}
