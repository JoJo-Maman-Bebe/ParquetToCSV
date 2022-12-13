using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileReaderAPI.Models
{
	public class BankingCountDetailsEventModel
	{
		public int BankingCountDetailsEventKey { get; set; }
		public int StoreNumber { get; set; }
		public string Date { get; set; }
		public string Time { get; set; }
		public int CountType { get; set; }
		public int CountedValue { get; set; }
		public int CurrencyCode { get; set; }
		public long PayrollNumber { get; set; }
	}
}