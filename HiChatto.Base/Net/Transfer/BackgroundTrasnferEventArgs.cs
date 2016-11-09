using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiChatto.Base.Net.Transfer
{
    public class BackgroundTrasnferEventArgs
    {
        public ResponseInfo ResponseInfomation { get; set; }
        public BackgroundTrasnferEventArgs()
        {
            ResponseInfomation = new ResponseInfo();
        }
        public BackgroundTrasnferEventArgs(ResponseInfo res)
        {
            this.ResponseInfomation = res;
        }
    }
    public delegate void BackgroundTransferDelegate(object sender, BackgroundTrasnferEventArgs e);
}
