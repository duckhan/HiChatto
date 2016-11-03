using System;

namespace HiChatto.Base.Net
{
    public class NetSourceEventArgs:EventArgs
    {
        public Package Package { get; set; }
        public byte[] Buffer { get; set; }
        public int BufferLength { get; set; }
        public object UserToken { get; set; }
        public NetSourceEventArgs()
        {
            Package = null;
            Buffer = null;
            UserToken = null;
            BufferLength = 0;
        }
        public NetSourceEventArgs(byte[] buffer,int buffLength,Package pkg)
        {
            Buffer = buffer;
            BufferLength = buffLength;
            Package = pkg;
        }
            
    }
}
