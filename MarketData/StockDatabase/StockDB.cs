using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

/*
    dotnet ef migrations add StockDatabaseCreate
    dotnet ef database update
*/
namespace MarketData.StockDatabase
{

    public class EquityInformationTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyId { get; set; }
        [Required]
        public string Symbol {get; set;}
        [Required]
        public string CompanyName {get; set;}
        [Required]
        public string ISINNumber {get; set;}
        [Required]
        public double FaceValue {get; set;}
        [Required]
        public int MarketLot {get; set;}
        [Required]
        public DateTime DateOfListing {get; set;}
        [Required]
        public double PaidUpValue {get; set;}
        [Required]
        public bool IsETF { get; set; }

        // Not mandatory Items
        public string Underlying { get; set; }
        public string Series {get; set;}

        // Items that dont need mapping
        [NotMapped]
        public String ETFName {
            get { return CompanyName;}
            set { CompanyName = value;}
        }
    }

    public class IndexInformationTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IndexId { get; set; }
        [Required]
        public string IndexName { get; set;}
    }

    public class CompanyToIndustryTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public string Industry { get; set; }
    }

    public class EquityBhavTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public int Day { get; set; }
        [Required]
        public double Close { get; set; }
        [Required]
        public double PrevClose { get; set; }
        [Required]
        public double TotalTradedQty { get; set; }
        [Required]
        public double TotalTradedValue { get; set; }
        [Required]
        public double TotalDeliveredQty { get; set; }
        [Required]
        public double TotalTrades { get; set; }
    }

    public class EquityOHLCTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public int Day { get; set; }
        [Required]
        public double Open { get; set; }
        [Required]
        public double Close { get; set; }
        [Required]
        public double High { get; set; }
        [Required]
        public double Low { get; set; }
        [Required]
        public double Last { get; set; }
        [Required]
        public double PrevClose { get; set; }
    }

    public class IndexBhavTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int IndexId { get; set; }
        [Required]
        public int Day { get; set; }
        [Required]
        public double Open { get; set; }
        [Required]
        public double Close { get; set; }
        [Required]
        public double High { get; set; }
        [Required]
        public double Low { get; set; }
        [Required]
        public double PointsChange { get; set; }
        [Required]
        public double PointsChangePct { get; set; }
        [Required]
        public double Volume { get; set; }
        [Required]
        public double TurnOver { get; set; }
        [Required]
        public double PE { get; set; }
        [Required]
        public double PB { get; set; }
        [Required]
        public double DivYield { get; set; }
    }

    public class CircuitBreakerTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int Day { get; set; }
        [Required]
        public int CompanyId { get; set; }
        [Required]
        public string HighLow { get; set; }
    }

    public class HighLow52WeekTable
    {
        [Key]
        public int CompanyId { get; set;}
        [Required]
        public double High { get; set;}
        [Required]
        public double Low { get; set;}

        public string UpDown30Days { get; set;}
    }

    class StockDataContext : DbContext
    {
        public DbSet<EquityInformationTable> CompanyInformation { get; set; }
        public DbSet<IndexInformationTable>  IndexInformation { get; set; }
        public DbSet<CompanyToIndustryTable> IndustryMapping { get; set; }
        public DbSet<EquityBhavTable>        EquityBhav { get; set; }
        public DbSet<EquityOHLCTable>        EquityOHLC { get; set; }
        public DbSet<IndexBhavTable>         IndexBhav { get; set; }
        public DbSet<CircuitBreakerTable>    CircuitBreaker { get; set; }
        public DbSet<HighLow52WeekTable>     HighLow52Week { get; set; }

        private string dbFilename;

        public StockDataContext()
        {
            dbFilename = "stockData.db";
            if(Globals.Options != null)
            {
                dbFilename = $"{Globals.Options.dbOptions.DBFolder}/{Globals.Options.dbOptions.DBFile}";
                if(!File.Exists(dbFilename))
                {
                    Globals.Log.Error($"DBFile {dbFilename} not found. Exiting with 1");
                    Environment.Exit(1);
                }
                else
                {
                    FileInfo s = new FileInfo(dbFilename);
                    Globals.Log.Info($"DbFile '{dbFilename}', Size: {s.Length/1024}KB, Modified: {s.LastWriteTime}");
                }
            }
        }

        public StockDataContext(DbContextOptions<StockDataContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = string.Format($"Data Source={dbFilename}");
            optionsBuilder.UseSqlite(connectionString);
            optionsBuilder.EnableSensitiveDataLogging(true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EquityInformationTable>().HasIndex(x => x.Symbol).IsUnique();
            modelBuilder.Entity<EquityInformationTable>().HasIndex(x => x.ISINNumber).IsUnique();
            modelBuilder.Entity<IndexInformationTable>().HasIndex(x => x.IndexName).IsUnique();
            modelBuilder.Entity<CompanyToIndustryTable>().HasIndex(x => new { x.CompanyId, x.Industry}).IsUnique();

            modelBuilder.Entity<EquityBhavTable>().HasIndex(x => x.CompanyId);
            modelBuilder.Entity<EquityBhavTable>().HasIndex(x => x.Day);
            modelBuilder.Entity<EquityBhavTable>().HasIndex(x => new { x.CompanyId, x.Day}).IsUnique();

            modelBuilder.Entity<EquityOHLCTable>().HasIndex(x => x.CompanyId);
            modelBuilder.Entity<EquityOHLCTable>().HasIndex(x => x.Day);
            modelBuilder.Entity<EquityOHLCTable>().HasIndex(x => new { x.CompanyId, x.Day}).IsUnique();

            modelBuilder.Entity<IndexBhavTable>().HasIndex(x => x.Day);
            modelBuilder.Entity<IndexBhavTable>().HasIndex(x => x.IndexId);
            modelBuilder.Entity<IndexBhavTable>().HasIndex(x => new {x.Day, x.IndexId}).IsUnique();

            modelBuilder.Entity<CircuitBreakerTable>().HasIndex(x => x.Day);
            modelBuilder.Entity<CircuitBreakerTable>().HasIndex(x => x.CompanyId);
            modelBuilder.Entity<CircuitBreakerTable>().HasIndex(x => new {x.CompanyId, x.Day}).IsUnique();

            modelBuilder.Entity<HighLow52WeekTable>().HasIndex(x => x.CompanyId).IsUnique();
        }

    }
}