using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileReaderAPI.Models
{
	public class BankingOverShortInvModel
	{
		public string MessageID { get; set; }
		public string VersionNo { get; set; }
		public int StoreNumber { get; set; }
		public string DiscrepancyDate { get; set; }
		public string DiscrepancyTime { get; set; }
		public string DiscrepancyLocation { get; set; }
		public string TenderMethodType { get; set; }
		public long DiscrepancyValue { get; set; }
		public string InvestigationEventDate { get; set; }
		public string InvestigationEventTime { get; set; }
		public long InvestigatorPayrollNumber { get; set; }
		public string InvestigationResponses { get; set; }
		public string UploadTime { get; set; }
		public long UniqueReferenceNumber { get; set; }
		public long AuthorisationPayrollNumber { get; set; }
		public string AuthorisationDate { get; set; }
		public string AuthorisationTime { get; set; }
		public string InvestigationNotes { get; set; }
		public string CurrencyCode { get; set; }
	}
}