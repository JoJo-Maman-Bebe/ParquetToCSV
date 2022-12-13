using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileReaderAPI.Models
{
	public class DgRegionModel
	{
		public int RegionCode { get; set; }
		public string RegionDesc { get; set; }
		public string CountryCode { get; set; }
		public string PartnerCode { get; set; }
	}
}