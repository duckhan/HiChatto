using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiChatto.Base
{
    public static class Helper
    {
        /// <summary>
        /// Convert Byte Array to String Hex Dump
        /// </summary>
        /// <param name="buff">Input</param>
        /// <param name="width">Nummer byte per line</param>
        /// <returns></returns>
        public static string ToHexDump(byte[] buff, int width = 20)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < buff.Length; i++)
            {
                if (i % width == 0)
                {
                    builder.AppendFormat("{0,4}:", i);
                }
                builder.AppendFormat("{0,3:X2}", buff[i]);
                if ((i + 1) % width == 0)
                {
                    builder.AppendLine();
                }
            }
            builder.AppendLine();
            return builder.ToString();
        }
    }
}
