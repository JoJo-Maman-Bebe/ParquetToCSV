using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileReaderAPI.Models
{
    public class TransactionErrorLogModel
    {

        public int TransactionNo { get; set; }
        public int BranchCode { get; set; }
        public string ExceptionMessage { get; set; }
        public string InnerExceptionMessage { get; set; }
    }
}