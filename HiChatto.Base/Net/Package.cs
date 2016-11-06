using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;

namespace HiChatto.Base.Net
{
    public class Package
    {
        public const int PackageSize = 8096;
        private byte[] _buff;
        protected int _code;
        public int Code
        {
            get { return _code; }
        }
        public byte[] Buffer
        {
            get { return _buff; }
        }
        /// <summary>
        /// Signs of Package in this App
        /// </summary>
        public const int Header = 696969;
        /// <summary>
        /// The length of Package's header
        /// Includes:
        /// 4 bytes Header
        /// 4 bytes Package's Code
        /// 4 bytes Client ID
        /// 4 bytes Package Length
        /// </summary>
        public const int HeaderSize = 16;
        protected int _offset;
        protected int _length;
        protected int _clientId;
        public int Length
        {
            get { return _length; }
        }
        public Package():this(0,0,8096)
        {

        }
        public Package(byte[] content)
        {
            _buff = content;
            ReadHeader();
        }
        public Package(int clientID,int code, int size)
        {
            _code = code;
            _buff = new byte[size];
            _length = 0;
            _offset = HeaderSize;
            _clientId = clientID;
        }
        public Package(int code) : this(0,code, PackageSize)
        {
        }
        public Package(ePackageType code) : this((int)code)
        {
        }
        public void WriteHeader()
        {
            int currentOffset = _offset;
            _offset = 0;
            WriteInt(Header);
            WriteInt(_code);
            WriteInt(_clientId);
            WriteInt(_length);
            _offset = currentOffset;
        }
        public bool ReadHeader()
        {
            try
            {
                int currentOffset = _offset;
                _offset = 0;
                int header = ReadInt();
                if (header != Header)
                {
                    _code = 0;
                    return false;
                }
                _code = ReadInt();
                _clientId = ReadInt();
                _length = ReadInt();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
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
        public void ResetOffset()
        {
            _offset = HeaderSize;
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
        public bool CopyFrom(byte[] buff,int offset,int count)
        {
            if (count<=_buff.Length && count - offset <= buff.Length)
            {
                Array.Copy(buff, offset, _buff, 0, count);
                return true;
            }
            return false;
        }
        public void WriteObject<T>(T obj) where T:class
        {
            DataContractSerializer d = new DataContractSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            d.WriteObject(ms, obj);
            ms.Position = 0;
            byte[] buff=ms.ToArray();
            WriteInt(buff.Length);
            WriteBytes(buff);
        }
        public T ReadObject<T>() where T:class
        {
            try
            {
                int len = ReadInt();

                DataContractSerializer d = new DataContractSerializer(typeof(T));
                MemoryStream ms = new MemoryStream();
                ms.Write(_buff, _offset, len);
                ms.Position = 0;
                object obj = d.ReadObject(ms);
                return obj as T;
            }
            catch(Exception)
            {
                return null;
            }
           
        }
    }
}
