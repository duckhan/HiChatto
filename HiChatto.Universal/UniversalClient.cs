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
        public UniversalClient(byte[] sendBuff, byte[] recieveBuff) : base(sendBuff, recieveBuff)
        {
            UniversalConfig config = new UniversalConfig();
            _ipe = new IPEndPoint(IPAddress.Parse(config.ServerIP), config.ServerPort);
        }

        public void Connect()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _rcEvent = new SocketAsyncEventArgs();
            _rcEvent.SetBuffer(_recieveBuffer,0,_recieveBuffer.Length);
            _rcEvent.Completed += RecieveAsync_Completed;
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
