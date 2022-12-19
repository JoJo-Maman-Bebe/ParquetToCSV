using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileReaderAPI.Models
{
	public class TenderCreditModel
	{

		public int TenderNo { get; set; }
		public long TransNo { get; set; }
		public string TenderAccNo { get; set; }
		public string TenderExp { get; set; }
		public string TenderAuthCode { get; set; }
		public string KeyWipeInd { get; set; }
		public string TenderAccVerNo { get; set; }
		public int TenderAmount { get; set; }
		public int TenderAmountBc { get; set; }
		public string AuthMethod { get; set; }
		public string ContactlessFormFactor { get; set; }
		public string PSPProvider { get; set; }
		public string PartnerCode { get; set; }

	}
}