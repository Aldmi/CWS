using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Dal.EfCore.Migrations
{
    public partial class init_create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeviceOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    ProduserUnionKey = table.Column<string>(maxLength: 256, nullable: false),
                    Description = table.Column<string>(nullable: false),
                    AutoBuild = table.Column<bool>(nullable: false),
                    ExchangeKeys = table.Column<string>(nullable: true),
                    MiddleWareMediator = table.Column<string>(nullable: true)
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
                    NumberErrorTrying = table.Column<int>(nullable: false),
                    NumberTimeoutTrying = table.Column<int>(nullable: false),
                    KeyTransport = table.Column<string>(nullable: true),
                    Provider = table.Column<string>(nullable: true),
                    CycleFuncOption = table.Column<string>(nullable: true)
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
                    AutoStartBg = table.Column<bool>(nullable: false),
                    DutyCycleTimeBg = table.Column<int>(nullable: false),
                    Headers = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HttpOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProduserUnionOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    ConverterName = table.Column<string>(nullable: true),
                    KafkaProduserOptions = table.Column<string>(nullable: true),
                    SignalRProduserOptions = table.Column<string>(nullable: true),
                    WebClientProduserOptions = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProduserUnionOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SerialPortOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Port = table.Column<string>(maxLength: 10, nullable: false),
                    BaudRate = table.Column<int>(nullable: false),
                    DataBits = table.Column<int>(nullable: false),
                    StopBits = table.Column<byte>(nullable: false),
                    Parity = table.Column<byte>(nullable: false),
                    DtrEnable = table.Column<bool>(nullable: false),
                    RtsEnable = table.Column<bool>(nullable: false),
                    AutoStartBg = table.Column<bool>(nullable: false),
                    DutyCycleTimeBg = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SerialPortOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StringInseartModelExt",
                columns: table => new
                {
                    Key = table.Column<string>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    Format = table.Column<string>(maxLength: 50, nullable: true),
                    BorderSubString = table.Column<string>(nullable: true),
                    StringHandlerMiddleWareOption = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StringInseartModelExt", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "TcpIpOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    IpAddress = table.Column<string>(nullable: false),
                    IpPort = table.Column<int>(nullable: false),
                    AutoStartBg = table.Column<bool>(nullable: false),
                    DutyCycleTimeBg = table.Column<int>(nullable: false)
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
                name: "ProduserUnionOptions");

            migrationBuilder.DropTable(
                name: "SerialPortOptions");

            migrationBuilder.DropTable(
                name: "StringInseartModelExt");

            migrationBuilder.DropTable(
                name: "TcpIpOptions");
        }
    }
}
