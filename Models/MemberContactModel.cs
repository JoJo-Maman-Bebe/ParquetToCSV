using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileReaderAPI.Models
{
	public class MemberContactModel
	{
		public string AccountNumber { get; set; }
		public string FirstName { get; set; }
		public string Surname { get; set; }
		public string  Address {get; set;}
		public string Address2 { get; set; }
		public string City { get; set; }
		public string PostCode { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public string MobilePhoneNumber { get; set; }
		public string Country { get; set; }
		public bool ContactViaEmail { get; set; }
		public bool ContactViaPhone { get; set; }
		public bool ContactViaPost { get; set; }
		public bool BlockAllContact { get; set; }
	}
}