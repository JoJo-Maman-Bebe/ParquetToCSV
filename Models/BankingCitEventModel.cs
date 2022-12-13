using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileReaderAPI.Models
{
	public class BankingCitEventModel
	{
		public string MessageID { get; set; }
		public string VersionNo { get; set; }
		public int StoreNumber { get; set; }
		public int MessageType { get; set; }
		public string BankingDate { get; set; }
		public int BankSlipNumber { get; set; }
		public string Tender { get; set; }
		public string Value { get; set; }
		public string BagNumber { get; set; }
		public string ReceiptNumber { get; set; }
		public string Status { get; set; }
		public long StoreBankerPayrollNumber { get; set; }
		public long StoreBankerCheckerPayrollNumber { get; set; }
		public string MessageActionDate { get; set; }
		public string MessageActionTime { get; set; }
		public long MessageActionPayrollNumber { get; set; }
		public long MessageActionCheckPayrollNumber { get; set; }
		public int TenderType { get; set; }
		public string CurrencyCode { get; set; }
	}
}