using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiChatto.Base.Net
{
    public class PackageHandlerAttribute:Attribute
    {
        private int _code;
        private string _desc;
        public int Code { get { return _code; } }
        public string Description { get { return _desc; } }
        public PackageHandlerAttribute(int code,string desc=null)
        {
            _code = code;
            _desc = desc;
        }
    }
}
