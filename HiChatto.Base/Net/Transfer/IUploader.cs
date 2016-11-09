using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiChatto.Base.Net.Transfer
{
    public interface IUploader
    {
        event BackgroundTransferDelegate Completed;
        Task<ResponseInfo> UploadAsync(string remoteUri, string[] files, bool toastNotify=true);
        Task<ResponseInfo> UploadAsync(string remoteUri, string file,bool toastNotify=true);
        void SetToastNotification(string xmlString);
    }
}
