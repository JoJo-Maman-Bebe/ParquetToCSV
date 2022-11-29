using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileReaderAPI.Models
{
	public class BankingOverShortModel
	{
		public string CompanyInd { get; set; }
		public string Branch { get; set; }
		public string BankDate { get; set; }
		public string SalesPerson { get; set; }
		public int SalesAsstNo { get; set; }
		public string Bank { get; set; }
		public string NoSaleReason { get; set; }
		public string TbvFlag { get; set; }
		public int TillNo { get; set; }
		public string TenderType { get; set; }
		public decimal BankAmount { get; set; }
		public decimal BankAmountBc { get; set; }
		public string PolledDate { get; set; }

	}
}