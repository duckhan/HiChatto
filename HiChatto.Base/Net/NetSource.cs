using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections.Generic;

namespace HiChatto.Base.Net
{
    public abstract class NetSource
    {
        protected bool _isConnected;
        protected bool _isSending;
        protected Queue<Package> _pkgQueue;
        public event NetSourceEventHandler Sent;
        public event NetSourceEventHandler Received;
        public event NetSourceEventHandler Connected;
        public event NetSourceEventHandler Disconnected;
        protected byte[] _sendBuffer;
        protected byte[] _recieveBuffer;
        public NetSource(byte[] sendBuff, byte[] recieveBuff)
        {
            _isConnected = false;
            _sendBuffer = sendBuff;
            _recieveBuffer = recieveBuff;
            _pkgQueue = new Queue<Package>();
        }
        public virtual void Send(Package pkg)
        {
            pkg.WriteHeader();
            Array.Copy(pkg.Buffer, 0, _sendBuffer, 4, pkg.Length);
            byte[] len = BitConverter.GetBytes(pkg.Length);
            for (int i = 0; i < 4; i++)
            {
                _sendBuffer[i] = len[i];
            }
            SendTCP(pkg.Length + 4, pkg);
        }
        protected abstract void SendTCP(int numBytes, Package pkg);
        protected virtual void OnRecieve(int numbytes)
        {
            int len = BitConverter.ToInt32(_recieveBuffer, 0);
            Package pkg = new Package();
            pkg.CopyFrom(_recieveBuffer, 4, len);
            pkg.ReadHeader();
            if (pkg.Code != 0)
            {
                OnRecievePackage(pkg);
            }
        }
        protected virtual void OnRecievePackage(Package pkg)
        {
            if (Received != null)
            {
                NetSourceEventArgs e = new NetSourceEventArgs(_recieveBuffer, pkg.Length + 4, pkg);
                Received(this, e);
            }
        }
        public abstract void Disconnect();
        public abstract void Connect();
        protected virtual void OnConnect()
        {
            if (_isConnected && Connected != null)
            {
                Connected(this, new NetSourceEventArgs());
            }
        }
        public void OnSent(NetSource sender, NetSourceEventArgs e)
        {
            if (Sent != null)
            {
                Sent(sender, e);
            }
        }
        protected virtual void OnDisconnect()
        {
            if (!_isConnected && Disconnected != null)
            {
                Disconnected(this, new NetSourceEventArgs());
            }
        }
    }
}
