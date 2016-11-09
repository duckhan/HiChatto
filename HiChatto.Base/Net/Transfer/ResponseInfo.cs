using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiChatto.Base.Net.Transfer
{
    public class ResponseInfo
    {
        public string Response { get; set; }
        public Uri RequestUri { get; set; }
        public uint StatusCode { get; set; }
        public IReadOnlyDictionary<string, string> Headers { get; set; }
    }
}
