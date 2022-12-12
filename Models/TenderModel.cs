using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileReaderAPI.Models
{
	public class TenderModel
	{

		public int TenderNo { get; set; }
		public long TransNo { get; set; }
		public int TenderType { get; set; }
		public int TenderAmount { get; set; }
		public int TenderAmountBc { get; set; }
	}
}