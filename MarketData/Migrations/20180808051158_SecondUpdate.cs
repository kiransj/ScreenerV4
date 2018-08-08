using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketData.Migrations
{
    public partial class SecondUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HighLow",
                table: "EquityOHLC",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HighLow",
                table: "EquityOHLC");
        }
    }
}
