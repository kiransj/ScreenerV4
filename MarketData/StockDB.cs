using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketData
{
    public class EquityInformation
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
        public int FaceValue {get; set;}
        [Required]
        public int MarketLot {get; set;}
        [Required]
        public DateTime DateOfListing {get; set;}

        // If the symbol is an ETF
        [Required]
        public bool IsETF {get; set;}
        [Required]
        public string Underlying {get; set;}

        // If the symbol is a security
        [Required]
        public string Series {get; set;}
        [Required]
        public string PaidUpValue {get; set;}
    }
    public class StockDB
    {
        StockDB()
        {


        }

    }
}