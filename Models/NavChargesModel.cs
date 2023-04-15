using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileReaderAPI.Models
{
    public class NavChargesModel
    {
        public string TransNo { get; set; }
        public string BranchCode { get; set; }
        public string SundryCode { get; set; }
        public string TransDate { get; set; }
        public string TransTime { get; set; }

        public decimal Amount { get; set; }
    }
}