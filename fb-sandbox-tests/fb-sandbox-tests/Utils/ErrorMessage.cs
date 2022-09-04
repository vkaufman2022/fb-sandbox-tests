using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace fbsandboxtests.Utils
{
    public class ErrorMessage
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public string? Error { get; set; }

        public override string ToString()
        {
            return StatusCode + "|" + Message + "|" +Error;
        }

    }
}
