using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileReaderAPI.Models
{
	public class StocktakeModel
	{
		public int Branch { get; set; }
		public string FrozenGroup { get; set; }
		public string Item { get; set; }
		public string Option { get; set; }
		public int Page { get; set; }
		public int BookQty { get; set; }
		public int ActualQty { get; set; }
		public string Group { get; set; }
		public string Status { get; set; }


	}
}