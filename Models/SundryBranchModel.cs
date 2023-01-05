using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileReaderAPI.Models
{
    public class SundryBranchModel
    {

        public int BranchCode { get; set; }
        public long TransNo { get; set; }
        public int SundryCode { get; set; }
        public int SundryValue { get; set; }
        public int SundryValueBc { get; set; }
        public string SundryReference { get; set; }
        public string SundryVoid { get; set; }
        public int SundryQuantity { get; set; }
    }
}