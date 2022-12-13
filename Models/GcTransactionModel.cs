using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileReaderAPI.Models
{
	public class GcTransactionModel
	{

		public int TransNo { get; set; }
		public string TransDate { get; set; }
		public int TransTime { get; set; }
		public int BranchCode { get; set; }
		public int TillNumber { get; set; }
		public string TransID { get; set; }
		public int PassFailInd { get; set; }
		public string GiftcardID { get; set; }
		public int SeqNo { get; set; }
		public int TransType { get; set; }
		public int PolledStatus { get; set; }
		public decimal Amount { get; set; }
		public int CurrencyID { get; set; }
		public decimal ExchangeRate { get; set; }
		public string OperatorID { get; set; }
		public string SupervisorID { get; set; }
		public int VoidIndicator { get; set; }
		public string NorFlag { get; set; }
		public int TransReason { get; set; }
		public string PolledDate { get; set; }
		public string DirectoryAccountNo { get; set; }
		public string Comments { get; set; }
		public string PartnerCode { get; set; }
		public string ClientID { get; set; }

	}
}