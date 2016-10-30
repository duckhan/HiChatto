using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HiChatto.Base.Config
{
    public abstract class BaseAppConfig
    {
        public BaseAppConfig()
        {

        }
        protected abstract void Load(Type type);
        protected object LoadPropertyAttribute(ConfigPropertyAttribute attri,string value=null)
        {
            if (string.IsNullOrEmpty(value))
            {
                return attri.Value;
            }
            else
            {
                return Convert.ChangeType(value, attri.Value.GetType());
            }
        }
    }
}
