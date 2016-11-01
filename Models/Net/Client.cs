using HiChatto.Base.Net;
using HiChatto.ClientLib.Models;
using System;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;

namespace HiChatto.ClientLib.Net
{
    public class Client : NetSource
    {
        StreamSocket _socket;
        private bool _isConnected;
        public bool IsConnected
        {
            get { return _isConnected; }
        }
        public Client(byte[] sendBuff, byte[] recieveBuff) : base(sendBuff, recieveBuff)
        {
        }

        public Task Connect(ClientConfig config)
        {
            try
            {
                _socket = new StreamSocket();
                return Task.Run(()=>_socket.ConnectAsync(new HostName(config.ServerIP), config.ServerPort.ToString()));

            }
            catch
            {
                _isConnected = false;
            }
            return null;
        }

        public void RecieveAsync()
        {
           
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
