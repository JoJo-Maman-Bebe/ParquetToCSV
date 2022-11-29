using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileReaderAPI.Models
{
	public class Warehouse740TransactionModel
	{
		public string RunDate { get; set; }
		public string ItemNo { get; set; }
		public string ItemOption { get; set; }
		public string CountryCode { get; set; }
		public int OrdersTakenQty { get; set; }
		public decimal OrdersTakenValue { get; set; }
		public int DespatchesQty { get; set; }
		public int ReturnsQty { get; set; }
		public decimal ReturnsValue { get; set; }
		public int CancelsQty { get; set; }
		public decimal CancelsValue { get; set; }
		public int VATRate { get; set; }
		public string ClientID { get; set; }

	}
}