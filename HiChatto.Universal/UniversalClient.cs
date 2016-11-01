using HiChatto.Base.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
namespace HiChatto.Universal
{
    public class UniversalClient : NetSource
    {
        IPEndPoint _ipe;
        Socket _socket;
        SocketAsyncEventArgs _rcEvent;
        public bool IsConnected
        {
            get { return _socket != null && _socket.Connected; }
        }
        public UniversalClient(byte[] sendBuff, byte[] recieveBuff) : base(sendBuff, recieveBuff)
        {
        }

        public void Connect(UniversalConfig config)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _ipe = new IPEndPoint(IPAddress.Parse(config.ServerIP), config.ServerPort);
            SocketAsyncEventArgs conn = new SocketAsyncEventArgs();
            conn.RemoteEndPoint = _ipe;
            conn.Completed += ConnectAsyncComleted;
            _rcEvent = new SocketAsyncEventArgs();
            _rcEvent.SetBuffer(_recieveBuffer,0,_recieveBuffer.Length);
            _rcEvent.Completed += RecieveAsync_Completed;
        }

        private void ConnectAsyncComleted(object sender, SocketAsyncEventArgs e)
        {
            Socket s = (Socket)sender;
            if (s.Connected && _connected !=null)
            {
                _connected(this, EventArgs.Empty);
            }
        }

        private void RecieveAsync_Completed(object sender, SocketAsyncEventArgs e)
        {
            RecieveAsync();
        }
        public void RecieveAsync()
        {
            _socket.ReceiveAsync(_rcEvent);
        }
        public override void Send(Package pkg)
        {
            throw new NotImplementedException();
        }

        protected override void LoadPackageHandler()
        {
            throw new NotImplementedException();
        }
    }
}
