using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileReaderAPI.Models
{
	public class StkMovementModel
	{
		public long SeqNo { get; set; }
		public string RecordType { get; set; }
		public string OwningCompany { get; set; }
		public string CompanyChain { get; set; }
		public string ProductGroup { get; set; }
		public string ItemNumber { get; set; }
		public string OptionNumber { get; set; }
		public string ReasonCode { get; set; }
		public string StockType { get; set; }
		public string DateProcessed { get; set; }
		public string TransactionDate { get; set; }
		public int Quantity { get; set; }
		public decimal CostValue { get; set; }
		public decimal RetailValue { get; set; }
		public int BranchCode { get; set; }
		public string TransferNoteNo { get; set; }
		public int ToFromBranch { get; set; }
		public string WarehouseCode { get; set; }
		public string LocationCode { get; set; }
		public string PickingListNo { get; set; }
		public string ConsignmentNo { get; set; }
		public string BoxsetNo { get; set; }
		public string CustOrderNo { get; set; }
		public string WrdNumber { get; set; }
		public decimal ActualSalesValue { get; set; }
		public int TransactionNo { get; set; }
		public string DocumentNo { get; set; }
		public string ToFromLocation { get; set; }
		public string BulletinNo { get; set; }
		public string CustOrderInd { get; set; }
		public string DespatchNoteNo { get; set; }
		public string PickTypeInd { get; set; }
		public string PickMethod { get; set; }

		public string RatioPackInd { get; set; }
		public string BoxsetSeqNo { get; set; }
		public string ProductPartnerCode { get; set; }
		public string BranchPartnerCode { get; set; }
	}
}