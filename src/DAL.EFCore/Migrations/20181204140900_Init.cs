using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.EFCore.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeviceOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    TopicName4MessageBroker = table.Column<string>(maxLength: 256, nullable: false),
                    Description = table.Column<string>(nullable: false),
                    AutoBuild = table.Column<bool>(nullable: false),
                    AutoStart = table.Column<bool>(nullable: false),
                    ExchangeKeysCollection = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExchangeOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Key = table.Column<string>(nullable: false),
                    AutoStartCycleFunc = table.Column<bool>(nullable: false),
                    NumberErrorTrying = table.Column<int>(nullable: false),
                    NumberTimeoutTrying = table.Column<int>(nullable: false),
                    KeyTransportMetaData = table.Column<string>(nullable: false),
                    ProviderOptionMetaData = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HttpOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    Address = table.Column<string>(maxLength: 256, nullable: false),
                    AutoStart = table.Column<bool>(nullable: false),
                    HeadersCollection = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HttpOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SerialPortOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Port = table.Column<string>(maxLength: 10, nullable: false),
                    BaudRate = table.Column<int>(nullable: false),
                    DataBits = table.Column<int>(nullable: false),
                    StopBits = table.Column<int>(nullable: false),
                    Parity = table.Column<int>(nullable: false),
                    DtrEnable = table.Column<bool>(nullable: false),
                    RtsEnable = table.Column<bool>(nullable: false),
                    AutoStart = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SerialPortOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TcpIpOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    IpAddress = table.Column<string>(nullable: false),
                    IpPort = table.Column<int>(nullable: false),
                    AutoStartBg = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TcpIpOptions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceOptions");

            migrationBuilder.DropTable(
                name: "ExchangeOptions");

            migrationBuilder.DropTable(
                name: "HttpOptions");

            migrationBuilder.DropTable(
                name: "SerialPortOptions");

            migrationBuilder.DropTable(
                name: "TcpIpOptions");
        }
    }
}
