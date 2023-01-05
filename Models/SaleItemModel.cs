using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileReaderAPI.Models
{
    public class SaleItemModel
    {

        public long TransNo { get; set; }
        public int SaleItemNo { get; set; }
        public int CompanyInd { get; set; }
        public string Company { get; set; }
        public string Chain { get; set; }
        public string DeptSubGroup { get; set; }
        public string ProductItemNo { get; set; }
        public int RealValue { get; set; }
        public int RealValueBc { get; set; }
        public int SaleValue { get; set; }
        public int SaleValueBc { get; set; }
        public int Quantity { get; set; }
        public int BranchCode { get; set; }
        public string TransDate { get; set; }
        public int TransTime { get; set; }
        public string PriceLookupInd { get; set; }
        public int SaleIndicator { get; set; }
        public int ReturnIndicator { get; set; }
        public string ReturnReasonCode { get; set; }
        public int VatRate { get; set; }
        public string FullUpos { get; set; }
        public string ULabel { get; set; }
        public int SeqNo { get; set; }
        public string ScannedKeyedInd { get; set; }
        public string GiftReceipt { get; set; }
        public int OrderIndicator { get; set; }
        public int PromoIndicator { get; set; }
        public int CorrectIndicator { get; set; }
        public int AllowIndicator { get; set; }
        public int DiscountIndicator { get; set; }
        public int NorIndicator { get; set; }
        public int MultibuyIndicator { get; set; }
        public string BranchOrig { get; set; }
        public string CustPostCode { get; set; }
        public string UposMatchOverride { get; set; }
        public string UposMatchOverrideBy { get; set; }
        public string GriReceiptType { get; set; }
        public string PartnerCode { get; set; }
    }
}