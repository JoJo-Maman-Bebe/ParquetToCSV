using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileReaderAPI.Models
{
	public class TransactionModel
	{
		public long TransNo { get; set; }
		public int TransID { get; set; }
		public int NoItems { get; set; }
		public int CompanyInd { get; set; }
		public int BranchCode { get; set; }
		public int PcNumber { get; set; }
		public int TillNumber { get; set; }
		public string TransDate { get; set; }
		public int TransTime { get; set; }
		public int SalesPerson { get; set; }
		public int SalesAsstNo { get; set; }
		public string VoidIndicator { get; set; }
		public string TaxFreeIndicator { get; set; }
		public int TransSaleValue { get; set; }
		public int TransSaleValueBc { get; set; }
		public int TransRealValue { get; set; }
		public int TransRealValueBc { get; set; }
		public int TransRevenue { get; set; }
		public int TransRevenueBc { get; set; }
		public string NoSaleReason { get; set; }
		public string TbvFlag { get; set; }
		public string PartnerCode { get; set; }


	}
}