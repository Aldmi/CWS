using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Dal.EfCore.Migrations
{
    public partial class RefactorinfDAL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutoStart",
                table: "DeviceOptions");

            migrationBuilder.RenameColumn(
                name: "AutoStart",
                table: "SerialPortOptions",
                newName: "AutoStartBg");

            migrationBuilder.RenameColumn(
                name: "AutoStart",
                table: "HttpOptions",
                newName: "AutoStartBg");

            migrationBuilder.AddColumn<int>(
                name: "DutyCycleTimeBg",
                table: "SerialPortOptions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DutyCycleTimeBg",
                table: "HttpOptions",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DutyCycleTimeBg",
                table: "SerialPortOptions");

            migrationBuilder.DropColumn(
                name: "DutyCycleTimeBg",
                table: "HttpOptions");

            migrationBuilder.RenameColumn(
                name: "AutoStartBg",
                table: "SerialPortOptions",
                newName: "AutoStart");

            migrationBuilder.RenameColumn(
                name: "AutoStartBg",
                table: "HttpOptions",
                newName: "AutoStart");

            migrationBuilder.AddColumn<bool>(
                name: "AutoStart",
                table: "DeviceOptions",
                nullable: false,
                defaultValue: false);
        }
    }
}
