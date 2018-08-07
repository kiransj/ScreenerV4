using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketData.Migrations
{
    public partial class StockDatabaseCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CircuitBreaker",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Day = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    HighLow = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CircuitBreaker", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyInformation",
                columns: table => new
                {
                    CompanyId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Symbol = table.Column<string>(nullable: false),
                    CompanyName = table.Column<string>(nullable: false),
                    ISINNumber = table.Column<string>(nullable: false),
                    FaceValue = table.Column<double>(nullable: false),
                    MarketLot = table.Column<int>(nullable: false),
                    DateOfListing = table.Column<DateTime>(nullable: false),
                    PaidUpValue = table.Column<double>(nullable: false),
                    IsETF = table.Column<bool>(nullable: false),
                    Underlying = table.Column<string>(nullable: true),
                    Series = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyInformation", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "EquityBhav",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyId = table.Column<int>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    Close = table.Column<double>(nullable: false),
                    PrevClose = table.Column<double>(nullable: false),
                    TotalTradedQty = table.Column<double>(nullable: false),
                    TotalTradedValue = table.Column<double>(nullable: false),
                    TotalDeliveredQty = table.Column<double>(nullable: false),
                    TotalTrades = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquityBhav", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquityOHLC",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyId = table.Column<int>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    Open = table.Column<double>(nullable: false),
                    Close = table.Column<double>(nullable: false),
                    High = table.Column<double>(nullable: false),
                    Low = table.Column<double>(nullable: false),
                    Last = table.Column<double>(nullable: false),
                    PrevClose = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquityOHLC", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HighLow52Week",
                columns: table => new
                {
                    CompanyId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    High = table.Column<double>(nullable: false),
                    Low = table.Column<double>(nullable: false),
                    UpDown30Days = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HighLow52Week", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "IndexBhav",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IndexId = table.Column<int>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    Open = table.Column<double>(nullable: false),
                    Close = table.Column<double>(nullable: false),
                    High = table.Column<double>(nullable: false),
                    Low = table.Column<double>(nullable: false),
                    PointsChange = table.Column<double>(nullable: false),
                    PointsChangePct = table.Column<double>(nullable: false),
                    Volume = table.Column<double>(nullable: false),
                    TurnOver = table.Column<double>(nullable: false),
                    PE = table.Column<double>(nullable: false),
                    PB = table.Column<double>(nullable: false),
                    DivYield = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndexBhav", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IndexInformation",
                columns: table => new
                {
                    IndexId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IndexName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndexInformation", x => x.IndexId);
                });

            migrationBuilder.CreateTable(
                name: "IndustryMapping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyId = table.Column<int>(nullable: false),
                    Industry = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndustryMapping", x => x.Id);
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

            migrationBuilder.CreateIndex(
                name: "IX_CompanyInformation_ISINNumber",
                table: "CompanyInformation",
                column: "ISINNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyInformation_Symbol",
                table: "CompanyInformation",
                column: "Symbol",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquityBhav_CompanyId",
                table: "EquityBhav",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_EquityBhav_Day",
                table: "EquityBhav",
                column: "Day");

            migrationBuilder.CreateIndex(
                name: "IX_EquityBhav_CompanyId_Day",
                table: "EquityBhav",
                columns: new[] { "CompanyId", "Day" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquityOHLC_CompanyId",
                table: "EquityOHLC",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_EquityOHLC_Day",
                table: "EquityOHLC",
                column: "Day");

            migrationBuilder.CreateIndex(
                name: "IX_EquityOHLC_CompanyId_Day",
                table: "EquityOHLC",
                columns: new[] { "CompanyId", "Day" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HighLow52Week_CompanyId",
                table: "HighLow52Week",
                column: "CompanyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IndexBhav_Day",
                table: "IndexBhav",
                column: "Day");

            migrationBuilder.CreateIndex(
                name: "IX_IndexBhav_IndexId",
                table: "IndexBhav",
                column: "IndexId");

            migrationBuilder.CreateIndex(
                name: "IX_IndexBhav_Day_IndexId",
                table: "IndexBhav",
                columns: new[] { "Day", "IndexId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IndexInformation_IndexName",
                table: "IndexInformation",
                column: "IndexName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IndustryMapping_CompanyId_Industry",
                table: "IndustryMapping",
                columns: new[] { "CompanyId", "Industry" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CircuitBreaker");

            migrationBuilder.DropTable(
                name: "CompanyInformation");

            migrationBuilder.DropTable(
                name: "EquityBhav");

            migrationBuilder.DropTable(
                name: "EquityOHLC");

            migrationBuilder.DropTable(
                name: "HighLow52Week");

            migrationBuilder.DropTable(
                name: "IndexBhav");

            migrationBuilder.DropTable(
                name: "IndexInformation");

            migrationBuilder.DropTable(
                name: "IndustryMapping");
        }
    }
}
