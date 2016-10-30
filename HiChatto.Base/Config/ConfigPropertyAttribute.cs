using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiChatto.Base.Config
{
    public class ConfigPropertyAttribute:Attribute
    {
        private string _key;
        private string _description;
        private object _value;
        public string Key
        {
            get { return _key; }
        }
        public string Description
        {
            get { return _description; }
        }
        public object Value
        {
            get { return _value; }
        }
        public ConfigPropertyAttribute(string key,object value,string desc = "")
        {
            _key = key;
            _value = value;
            _description = desc;
        }
    }
}
