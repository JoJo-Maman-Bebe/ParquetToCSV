using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileReaderAPI.Models
{
    public class ApiLogModel
    {
        public DateTime LastActivity { get; set; }
        public string ApiName { get; set; }
        
        public string LastResult { get; set; }

    }
}