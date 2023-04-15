using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileReaderAPI.Models
{
    public class NavSalesEntryModel
    {
        public decimal VatPercentage { get; set; }
        public string ReceiptNumber { get; set; }

        public string NextItemOption { get; set; }

        public int SaleItemNo { get; set; }

        public string Date { get; set; }

        public int Quantity { get; set; }

        public decimal SaleValue { get; set; }

        public string SalesPerson { get; set; }
        
        public string Time { get; set; }

        public string ScannedKeyInd { get; set; }

        public bool PromoIndicator { get; set; }

        public bool DiscountIndicator { get; set; }
        public string BranchCode { get; set; }
        


    }
}