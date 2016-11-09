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
        void UploadAsync(string remoteUri, string[] files);
        void UploadAsync(string remoteUri, string file);
        void SetToastNotification(string xmlString);
    }
}
