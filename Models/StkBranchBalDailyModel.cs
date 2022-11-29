using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileReaderAPI.Models
{
	public class StkBranchBalDailyModel
	{

		public string OpeningDate { get; set; }
		public string OwningCompany { get; set; }
		public string SeasonCode { get; set; }
		public int BranchCode { get; set; }
		public string CompanyChain { get; set; }
		public string ProductGroup { get; set; }
		public string ItemNumber { get; set; }
		public string OptionNumber { get; set; }
		public int FsUnits { get; set; }
		public decimal FsCostVal { get; set; }

	}
}