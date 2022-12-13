using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileReaderAPI.Models
{
	public class ItemDiscountModel
	{
	public int SaleItemNo { get; set; }
	public long TransNo  { get; set; }
	public int DiscountValue { get; set; }
	public int DiscountValueBc { get; set; }
	public int DiscountRate { get; set; }
	public int DiscountCardID { get; set; }
	public int DiscountCardNo { get; set; }
	public int DiscountCardVer { get; set; }
	public string DiscountKeyWipe { get; set; }
	public string DisUniformInd { get; set; }
}
}