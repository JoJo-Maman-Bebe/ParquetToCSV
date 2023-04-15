using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileReaderAPI.Models
{
    public class NavTransactionStatusModel
    {

        public int TransactionNumber { get; set; }
        public int BranchCode { get; set; }
        public int HeaderInserted { get; set; }
        public int SalesEntryInserted { get; set; }
        public int PaymentEntryInserted { get; set; }
        public int BankCountAdded { get; set; }

        public int NavChargeAdded { get; set; }
    }
}