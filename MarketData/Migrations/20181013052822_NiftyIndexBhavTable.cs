using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketData.Migrations
{
    public partial class NiftyIndexBhavTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NiftyIndexBhav",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IndexName = table.Column<string>(nullable: false),
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
                    table.PrimaryKey("PK_NiftyIndexBhav", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NiftyIndexBhav_Day",
                table: "NiftyIndexBhav",
                column: "Day");

            migrationBuilder.CreateIndex(
                name: "IX_NiftyIndexBhav_IndexName",
                table: "NiftyIndexBhav",
                column: "IndexName");

            migrationBuilder.CreateIndex(
                name: "IX_NiftyIndexBhav_Day_IndexName",
                table: "NiftyIndexBhav",
                columns: new[] { "Day", "IndexName" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NiftyIndexBhav");
        }
    }
}
