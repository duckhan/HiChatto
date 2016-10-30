using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiChatto.Base.Net
{
    public class Package
    {
        public const int DEFAULT_SIZE = 2048;
        private byte[] _buff;
        protected int _code;
        public int Code
        {
            get { return _code; }
        }
        public const int HEADER = 696969;
        protected int _offset;
        protected int _length;
        public Package(int code, int size)
        {
            _code = code;
            _buff = new byte[size];
            _length = 0;
            _offset = 0;
        }
        public Package(int code) : this(code, DEFAULT_SIZE)
        {

        }
        public void WriteBytes(byte[] buff, int offset, int length)
        {
            if (length + _offset > _buff.Length)
            {
                byte[] tmp = _buff;
                _buff = new byte[_buff.Length * 2];
                WriteBytes(buff, offset, length);
            }
            Array.Copy(buff, offset, _buff, _offset, length);
            _offset += length;
            _length = (_offset > _length) ? _offset : _length;
        }
        public void WriteBytes(byte[] buff)
        {
            WriteBytes(buff, 0, buff.Length);
        }
        public void WriteByte(byte val)
        {
            if (_offset >= _buff.Length)
            {
                byte[] tmp = _buff;
                _buff = new byte[_buff.Length * 2];
                Array.Copy(tmp, _buff, tmp.Length);
            }
            _buff[_offset++] = val;
            _length = (_offset > _length) ? _offset : _length;
        }
        public void WriteShort(short val)
        {
            WriteBytes(BitConverter.GetBytes(val));
        }
        public void WriteInt(int val)
        {
            WriteBytes(BitConverter.GetBytes(val));
        }
        public void WriteLong(long val)
        {
            WriteBytes(BitConverter.GetBytes(val));
        }
        public void WriteString(string val)
        {
            if (!string.IsNullOrEmpty(val))
            {
                byte[] buff = UnicodeEncoding.UTF8.GetBytes(val);
                WriteInt(buff.Length + 1);//Lenght of String
                WriteBytes(buff);
                WriteByte(0);//End of String
            }
            else
            {
                WriteInt(1);
                WriteByte(0);
            }
        }
        public byte[] ReadBytes(int len)
        {
            byte[] buff = new byte[len];
            Array.Copy(_buff, _offset, buff, 0, len);
            _offset += len;
            return buff;
        }
        public byte ReadByte()
        {
            return _buff[_offset++];
        }
        public short ReadShort()
        {
            short ret = BitConverter.ToInt16(_buff, _offset);
            _offset += 2;
            return ret;
        }
        public int ReadInt()
        {
            int ret = BitConverter.ToInt32(_buff, _offset);
            _offset += 4;
            return ret;
        }
        public long ReadLong()
        {
            long ret = BitConverter.ToInt64(_buff, _offset);
            _offset += 8;
            return ret;
        }
        public string ReadString()
        {
            int len = ReadInt();
            byte[] buff = ReadBytes(len);
            return UnicodeEncoding.UTF8.GetString(buff,0,len);
        }
    }
}
