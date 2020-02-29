using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Dal.EfCore.Migrations
{
    public partial class AddJsonAttribute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KafkaProduserOptionsMetaData",
                table: "ProduserUnionOptions");

            migrationBuilder.DropColumn(
                name: "SignalRProduserOptionsMetaData",
                table: "ProduserUnionOptions");

            migrationBuilder.DropColumn(
                name: "WebClientProduserOptionsMetaData",
                table: "ProduserUnionOptions");

            migrationBuilder.DropColumn(
                name: "HeadersCollection",
                table: "HttpOptions");

            migrationBuilder.DropColumn(
                name: "CycleFuncOptionMetaData",
                table: "ExchangeOptions");

            migrationBuilder.DropColumn(
                name: "KeyTransportMetaData",
                table: "ExchangeOptions");

            migrationBuilder.DropColumn(
                name: "ProviderOptionMetaData",
                table: "ExchangeOptions");

            migrationBuilder.DropColumn(
                name: "ExchangeKeysCollection",
                table: "DeviceOptions");

            migrationBuilder.DropColumn(
                name: "MiddleWareInDataMetaData",
                table: "DeviceOptions");

            migrationBuilder.AddColumn<string>(
                name: "KafkaProduserOptions",
                table: "ProduserUnionOptions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SignalRProduserOptions",
                table: "ProduserUnionOptions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebClientProduserOptions",
                table: "ProduserUnionOptions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Headers",
                table: "HttpOptions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CycleFuncOption",
                table: "ExchangeOptions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KeyTransport",
                table: "ExchangeOptions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Provider",
                table: "ExchangeOptions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExchangeKeys",
                table: "DeviceOptions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleWareInData",
                table: "DeviceOptions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KafkaProduserOptions",
                table: "ProduserUnionOptions");

            migrationBuilder.DropColumn(
                name: "SignalRProduserOptions",
                table: "ProduserUnionOptions");

            migrationBuilder.DropColumn(
                name: "WebClientProduserOptions",
                table: "ProduserUnionOptions");

            migrationBuilder.DropColumn(
                name: "Headers",
                table: "HttpOptions");

            migrationBuilder.DropColumn(
                name: "CycleFuncOption",
                table: "ExchangeOptions");

            migrationBuilder.DropColumn(
                name: "KeyTransport",
                table: "ExchangeOptions");

            migrationBuilder.DropColumn(
                name: "Provider",
                table: "ExchangeOptions");

            migrationBuilder.DropColumn(
                name: "ExchangeKeys",
                table: "DeviceOptions");

            migrationBuilder.DropColumn(
                name: "MiddleWareInData",
                table: "DeviceOptions");

            migrationBuilder.AddColumn<string>(
                name: "KafkaProduserOptionsMetaData",
                table: "ProduserUnionOptions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SignalRProduserOptionsMetaData",
                table: "ProduserUnionOptions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebClientProduserOptionsMetaData",
                table: "ProduserUnionOptions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeadersCollection",
                table: "HttpOptions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CycleFuncOptionMetaData",
                table: "ExchangeOptions",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "KeyTransportMetaData",
                table: "ExchangeOptions",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProviderOptionMetaData",
                table: "ExchangeOptions",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExchangeKeysCollection",
                table: "DeviceOptions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleWareInDataMetaData",
                table: "DeviceOptions",
                nullable: true);
        }
    }
}
