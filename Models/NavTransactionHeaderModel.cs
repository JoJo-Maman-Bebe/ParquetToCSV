using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileReaderAPI.Models
{
    public class NavTransactionHeaderModel
    {
		public string ReceiptNumber { get; set; }

		public string BranchCode { get; set; }

		public string StaffID { get; set; }

		public string Date { get; set; }

		public string Time { get; set; }

		public decimal SaleValue { get; set; }

		//public decimal DiscountAmount { get; set; }

		public decimal RealValue { get; set; }

		public char VoidIndicator { get; set; }

	}
}