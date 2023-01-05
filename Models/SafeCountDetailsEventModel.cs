using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileReaderAPI.Models
{
    public class SafeCountDetailsEventModel
    {
        public int SafeCountDetailsEventKey { get; set; }
        public int StoreNumber { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public int CountedValue { get; set; }
        public int DiscrepancyAmount { get; set; }
        public string CurrencyCode { get; set; }

        public long PayrollNumber { get; set; }

    }
}