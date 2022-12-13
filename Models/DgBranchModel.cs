using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileReaderAPI.Models
{
	public class DgBranchModel
	{
		public int BranchCode { get; set; }
		public string BranchDesc { get; set; }
		public int AreaCode { get; set; }
		public string BranchAddress1 { get; set; }
		public string BranchAddress2 { get; set; }
		public string BranchAddress3 { get; set; }
		public string BranchAddress4 { get; set; }
		public string PostCode { get; set; }
		public string Telephone { get; set; }
		public int NoTills { get; set; }
		public int BranchSqFeet { get; set; }
		public string BranchCoChain { get; set; }
		public string OpeningDate { get; set; }
		public string ClosingDate { get; set; }
		public string CountryCode { get; set; }
		public string GlCompanyCode { get; set; }
		public string FranchisePartner { get; set; }
		public string FranBulkOrRepl { get; set; }
		public string SatOpeningFlag { get; set; }
		public string MerSwiaccvisa { get; set; }
		public string MerAmex { get; set; }
		public string MerDiners { get; set; }
		public string MerClub24 { get; set; }
		public string MerTime { get; set; }
		public string MerJcb { get; set; }
		public string MerStyle { get; set; }
		public string AlcoholLicense { get; set; }
		public string AlOpeningMon { get; set; }
		public string AlOpeningTue { get; set; }
		public string AlOpeningWed { get; set; }
		public string AlOpeningThu { get; set; }
		public string AlOpeningFri { get; set; }
		public string AlClosingFri { get; set; }
		public string AlClosingSat { get; set; }
		public string AlClosingSun { get; set; }
		public string BranchType { get; set; }
		public int CompanyInd { get; set; }
		public string PartnerCode { get; set; }

	}
}