using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketData.Migrations
{
    public partial class StockDatabaseNiftyOptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NiftyBhav",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OptionId = table.Column<int>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    ExpDay = table.Column<int>(nullable: false),
                    StrikePrice = table.Column<double>(nullable: false),
                    CallOption = table.Column<bool>(nullable: false),
                    Open = table.Column<double>(nullable: false),
                    Close = table.Column<double>(nullable: false),
                    High = table.Column<double>(nullable: false),
                    Low = table.Column<double>(nullable: false),
                    OpenIntrest = table.Column<double>(nullable: false),
                    TradedQty = table.Column<double>(nullable: false),
                    NumOfCont = table.Column<double>(nullable: false),
                    NumOfTrade = table.Column<double>(nullable: false),
                    NotionalValue = table.Column<double>(nullable: false),
                    PrVal = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NiftyBhav", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NiftyBhav_Day",
                table: "NiftyBhav",
                column: "Day");

            migrationBuilder.CreateIndex(
                name: "IX_NiftyBhav_ExpDay_OptionId",
                table: "NiftyBhav",
                columns: new[] { "ExpDay", "OptionId" });

            migrationBuilder.CreateIndex(
                name: "IX_NiftyBhav_ExpDay_StrikePrice",
                table: "NiftyBhav",
                columns: new[] { "ExpDay", "StrikePrice" });

            migrationBuilder.CreateIndex(
                name: "IX_NiftyBhav_ExpDay_OptionId_CallOption_StrikePrice",
                table: "NiftyBhav",
                columns: new[] { "ExpDay", "OptionId", "CallOption", "StrikePrice" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NiftyBhav");
        }
    }
}
