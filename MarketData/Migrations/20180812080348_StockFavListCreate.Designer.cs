﻿// <auto-generated />
using System;
using MarketData.StockDatabase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MarketData.Migrations
{
    [DbContext(typeof(StockDataContext))]
    [Migration("20180812080348_StockFavListCreate")]
    partial class StockFavListCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846");

            modelBuilder.Entity("MarketData.StockDatabase.CompanyToIndustryTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CompanyId");

                    b.Property<string>("Industry")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CompanyId", "Industry")
                        .IsUnique();

                    b.ToTable("IndustryMapping");
                });

            modelBuilder.Entity("MarketData.StockDatabase.EquityBhavTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Close");

                    b.Property<int>("CompanyId");

                    b.Property<int>("Day");

                    b.Property<double>("PrevClose");

                    b.Property<double>("TotalDeliveredQty");

                    b.Property<double>("TotalTradedQty");

                    b.Property<double>("TotalTradedValue");

                    b.Property<double>("TotalTrades");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("Day");

                    b.HasIndex("CompanyId", "Day")
                        .IsUnique();

                    b.ToTable("EquityBhav");
                });

            modelBuilder.Entity("MarketData.StockDatabase.EquityInformationTable", b =>
                {
                    b.Property<int>("CompanyId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompanyName")
                        .IsRequired();

                    b.Property<DateTime>("DateOfListing");

                    b.Property<double>("FaceValue");

                    b.Property<string>("ISINNumber")
                        .IsRequired();

                    b.Property<bool>("IsETF");

                    b.Property<int>("MarketLot");

                    b.Property<double>("PaidUpValue");

                    b.Property<string>("Series");

                    b.Property<string>("Symbol")
                        .IsRequired();

                    b.Property<string>("Underlying");

                    b.HasKey("CompanyId");

                    b.HasIndex("ISINNumber")
                        .IsUnique();

                    b.HasIndex("Symbol")
                        .IsUnique();

                    b.ToTable("CompanyInformation");
                });

            modelBuilder.Entity("MarketData.StockDatabase.EquityOHLCTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Close");

                    b.Property<int>("CompanyId");

                    b.Property<int>("Day");

                    b.Property<double>("High");

                    b.Property<string>("HighLow")
                        .IsRequired();

                    b.Property<double>("Last");

                    b.Property<double>("Low");

                    b.Property<double>("Open");

                    b.Property<double>("PrevClose");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("Day");

                    b.HasIndex("CompanyId", "Day")
                        .IsUnique();

                    b.ToTable("EquityOHLC");
                });

            modelBuilder.Entity("MarketData.StockDatabase.HighLow52WeekTable", b =>
                {
                    b.Property<int>("CompanyId")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("High");

                    b.Property<double>("Low");

                    b.Property<string>("UpDown30Days");

                    b.HasKey("CompanyId");

                    b.HasIndex("CompanyId")
                        .IsUnique();

                    b.ToTable("HighLow52Week");
                });

            modelBuilder.Entity("MarketData.StockDatabase.IndexBhavTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Close");

                    b.Property<int>("Day");

                    b.Property<double>("DivYield");

                    b.Property<double>("High");

                    b.Property<int>("IndexId");

                    b.Property<double>("Low");

                    b.Property<double>("Open");

                    b.Property<double>("PB");

                    b.Property<double>("PE");

                    b.Property<double>("PointsChange");

                    b.Property<double>("PointsChangePct");

                    b.Property<double>("TurnOver");

                    b.Property<double>("Volume");

                    b.HasKey("Id");

                    b.HasIndex("Day");

                    b.HasIndex("IndexId");

                    b.HasIndex("Day", "IndexId")
                        .IsUnique();

                    b.ToTable("IndexBhav");
                });

            modelBuilder.Entity("MarketData.StockDatabase.IndexInformationTable", b =>
                {
                    b.Property<int>("IndexId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("IndexName")
                        .IsRequired();

                    b.HasKey("IndexId");

                    b.HasIndex("IndexName")
                        .IsUnique();

                    b.ToTable("IndexInformation");
                });

            modelBuilder.Entity("MarketData.StockDatabase.StockFavList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ListName")
                        .IsRequired();

                    b.Property<string>("Symbol")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Symbol", "ListName")
                        .IsUnique();

                    b.ToTable("Favlist");
                });
#pragma warning restore 612, 618
        }
    }
}
