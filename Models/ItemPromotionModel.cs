using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileReaderAPI.Models
{
	public class ItemPromotionModel
	{

		public int SaleItemNo { get; set; }
		public int TransNo { get; set; }
		public int PromotionCode { get; set; }
		public int DiscountValue { get; set; }
		public int DiscountValueBc { get; set; }
		public int DiscountRate { get; set; }

	}
}