using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileReaderAPI.Models
{
	public class TillCountDetailsEventModel
	{

		public int TillCountKey  { get; set; }
		public int StoreNumber { get; set; }
		public string Date { get; set; }
		public string Time { get; set; }
		public int TillNumber { get; set; }
		public int NumberOfTills { get; set; }
		public int CountedValue { get; set; }
		public int DiscrepancyAmount { get; set; }

		public int TenderMethodType { get; set; }
		public string CurrencyCode { get; set; }
	}
}