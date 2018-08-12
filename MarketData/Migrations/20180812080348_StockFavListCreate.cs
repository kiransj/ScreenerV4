using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketData.Migrations
{
    public partial class StockFavListCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CircuitBreaker");

            migrationBuilder.CreateTable(
                name: "Favlist",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Symbol = table.Column<string>(nullable: false),
                    ListName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favlist", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Favlist_Symbol_ListName",
                table: "Favlist",
                columns: new[] { "Symbol", "ListName" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favlist");

            migrationBuilder.CreateTable(
                name: "CircuitBreaker",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyId = table.Column<int>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    HighLow = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CircuitBreaker", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CircuitBreaker_CompanyId",
                table: "CircuitBreaker",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CircuitBreaker_Day",
                table: "CircuitBreaker",
                column: "Day");

            migrationBuilder.CreateIndex(
                name: "IX_CircuitBreaker_CompanyId_Day",
                table: "CircuitBreaker",
                columns: new[] { "CompanyId", "Day" },
                unique: true);
        }
    }
}
