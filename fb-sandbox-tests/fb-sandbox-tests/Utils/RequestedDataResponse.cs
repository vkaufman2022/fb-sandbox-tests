using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace fbsandboxtests.Utils
{
    public class RequestedDataResponse
    {
        public string? Msg { get; set; }
        public ReqData? ReqData { get; set; }
        public string? TokenReceived { get; set; }
        public int StatusCode { get; set; }

        public override string ToString()
        {
            return Msg + "|" + ReqData?.Url + "|" + ReqData?.Method + "|"+ ReqData?.Host +"|"+ TokenReceived + "|" + StatusCode;
        }

    }
    public class ReqData
    {
        public string? Url { get; set; }
        public string? Method { get; set; }
        public string? Host { get; set; }
    }

}
