using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace HiChatto.Base.Net
{
    public abstract class NetSource
    {
        protected EventHandler _connected;
        protected EventHandler _disconnected;
        public event EventHandler Connected
        {
            add { _connected += value; }
            remove { _connected -= value; }
        }
        public event EventHandler Disconnected
        {
            add { _disconnected += value; }
            remove { _disconnected -= value; }
        }
        protected static IPackageHandler[] _handlers;
        protected byte[] _sendBuffer;
        protected byte[] _recieveBuffer;
        public NetSource(byte[] sendBuff,byte[] recieveBuff)
        {
            _sendBuffer = sendBuff;
            _recieveBuffer = recieveBuff;
        }
        public abstract void Send(Package pkg);
        protected void OnRecieve(int numbytes)
        {
        }
        protected void OnRecievePackage(Package pkg)
        {
            try
            {
                if (pkg.Code < _handlers.Length)
                {
                    _handlers[pkg.Code].Handle(this, pkg);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        protected abstract void LoadPackageHandler();
    }
}
