using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileReaderAPI.Models
{
	public class ItemOptionBarcodeModel
	{
		public int RowID { get; set; }
		public string ItemNumber { get; set; }
		public string OptionNumber { get; set; }
		public string Barcode { get; set; }
		public bool IsDeleted { get; set; }
		public string FulfilCoArticleId { get; set; }

	}
}