using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiChatto.Base;
using System.Threading;
namespace HiChatto.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Server sv = new Server();
            sv.Start();
            Console.ReadLine();
        }
    }
}
