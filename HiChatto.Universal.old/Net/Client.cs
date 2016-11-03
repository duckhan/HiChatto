using HiChatto.Models;
using HiChatto.Base.Net;
using System;
namespace HiChatto.Universal.Net
{
    public class Client : NetSource
    {
        Socket _socket;
        private bool _isConnected;
        public bool IsConnected
        {
            get { return _isConnected; }
        }
        SocketAsyncEventArgs receiveEvent;
        public Client() : base(new byte[8096], new byte[8096])
        {

        }

        private void ReceiveAsynCompleted(object sender, SocketAsyncEventArgs e)
        {
            throw new NotImplementedException();
        }

        public Task Connect(ClientConfig config)
        {
            try
            {
                SocketAsyncEventArgs conEv = new SocketAsyncEventArgs();
                receiveEvent = new SocketAsyncEventArgs();
                receiveEvent.Completed += ReceiveAsynCompleted;
                receiveEvent.SetBuffer(_recieveBuffer, 0, _recieveBuffer.Length);
                conEv.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(config.ServerIP), config.ServerPort);
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _socket.ConnectAsync(conEv);
                conEv.Completed += ConnectAsyncComplete;             
            }
            catch
            {
                _isConnected = false;
            }
            return null;
        }

        private void ConnectAsyncComplete(object sender, SocketAsyncEventArgs e)
        {
            _connected?.Invoke(this, e);
        }

        public void RecieveAsync()
        {
            try
            {
                _socket.ReceiveAsync(receiveEvent);
            }
            catch
            {
                _disconnected?.Invoke(this, EventArgs.Empty);
            }
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
