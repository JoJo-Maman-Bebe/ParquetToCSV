using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileReaderAPI.Models
{
    public class NavPaymentEntryModel
    {
        public string TenderType { get; set; }
        public decimal TenderAmount { get; set; }

        public decimal AmountInCurrency { get; set; }

        public string Date { get; set; }
        public string Time { get; set; }

        public string SalesPerson { get; set; }
        public string StoreNo { get; set; }
        public string ReceiptNo { get; set; }

        public int SaleItemNo { get; set; }

        public string NavCardNo { get; set; }
    }
}