using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Dal.EfCore.Migrations
{
    public partial class test11111 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StringInseartModelExt",
                table: "StringInseartModelExt");

            migrationBuilder.AlterColumn<string>(
                name: "VarName",
                table: "StringInseartModelExt",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StringInseartModelExt",
                table: "StringInseartModelExt",
                column: "VarName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StringInseartModelExt",
                table: "StringInseartModelExt");

            migrationBuilder.AlterColumn<string>(
                name: "VarName",
                table: "StringInseartModelExt",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_StringInseartModelExt",
                table: "StringInseartModelExt",
                column: "Id");
        }
    }
}
