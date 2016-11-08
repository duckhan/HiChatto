using HiChatto.Base.Net;
using System;
using System.Net.Sockets;
using System.Net;
using HiChatto.Models;
namespace HiChatto.Universal.Net
{
    public class UniversalClient : NetSource
    {
        Socket _socket;
        UserInfo _user;
        public UserInfo User
        {
            get { return _user; }
            set
            {
                _user = value;
            }
        }
        ClientConfig _config;
        public ClientConfig Config { get { return _config; } }
        public Socket Socket { get { return _socket; } }
        public bool IsConnected
        {
           
            get { return _isConnected && _socket != null && _socket.Connected; }
        }

        SocketAsyncEventArgs receiveEvent;
        public UniversalClient(ClientConfig config) : base(new byte[8096], new byte[8096])
        {
            _config = config;
            _user = new UserInfo();
        }

        private void ReceiveAsynCompleted(object sender, SocketAsyncEventArgs e)
        {
            OnRecieve(e.BytesTransferred);
            RecieveAsync();
        }

        private void ConnectAsyncComplete(object sender, SocketAsyncEventArgs e)
        {
            _isConnected = true;
            OnConnect();
        }

        public void RecieveAsync()
        {
            try
            {
                _socket.ReceiveAsync(receiveEvent);
            }
            catch
            {
                _isConnected = false;
                Disconnect();
            }
        }

        protected override void SendTCP(int numBytes, Package pkg)
        {
            try
            {
                if (_socket != null && _socket.Connected)
                {
                    lock (_pkgQueue)
                    {
                        _pkgQueue.Enqueue(pkg);
                        if (_isSending)
                        {
                            return;
                        }
                        else
                        {
                            _isSending = true;
                            pkg = _pkgQueue.Dequeue();
                        }
                        SocketAsyncEventArgs e = new SocketAsyncEventArgs();
                        e.SetBuffer(_sendBuffer, 0, numBytes);
                        e.UserToken = pkg;
                        e.Completed += SendAsyncCompleted;
                        _socket.SendAsync(e);
                    }
                  
                }
            }
            catch
            {
                Disconnect();
            }
        }
        private void SendAsyncCompleted(object sender, SocketAsyncEventArgs e)
        {
            NetSourceEventArgs sentEvent = new NetSourceEventArgs();
            sentEvent.UserToken = e;
            OnSent(this, sentEvent);
            _isSending = false;
            if (_pkgQueue.Count > 0)
            {
                Package pkg = _pkgQueue.Dequeue();
                SendTCP(pkg.Length + 4, pkg);
            }
        }

        public override void Disconnect()
        {
            if (_isConnected)
            {
                _isConnected = false;
                _socket.Dispose();
                receiveEvent.Dispose();
                OnDisconnect();
            }
        }

        public override void Connect()
        {
            try
            {
                SocketAsyncEventArgs conEv = new SocketAsyncEventArgs();
                receiveEvent = new SocketAsyncEventArgs();
                receiveEvent.Completed += ReceiveAsynCompleted;
                receiveEvent.SetBuffer(_recieveBuffer, 0, _recieveBuffer.Length);
                conEv.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(_config.ServerIP), _config.ServerPort);
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                conEv.Completed += ConnectAsyncComplete;
                _socket.ConnectAsync(conEv);
                
            }
            catch
            {
                _isConnected = false;
            }
        }
    }
}
