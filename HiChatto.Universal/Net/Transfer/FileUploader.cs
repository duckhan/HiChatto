using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using HiChatto.Base.Net.Transfer;
using Windows.Storage;
using Windows.Storage.Streams;
using System.IO;

namespace HiChatto.Universal.Net.Transfer
{
    public class FileUploader : IUploader
    {
        BackgroundUploader uploader;
        public FileUploader()
        {
            uploader = new BackgroundUploader();
            uploader.Method = "POST";
        }
        private BackgroundTransferDelegate completed;
        public event BackgroundTransferDelegate Completed
        {
            add { completed += value; }
            remove { completed -= value; }
        }

        public async void UploadAsync(string remoteUri, string file)
        {
            StorageFile storageFile = await StorageFile.GetFileFromPathAsync(file);
            if (storageFile != null)
            {
                BackgroundTransferContentPart part = new BackgroundTransferContentPart("file", storageFile.Name);
                part.SetFile(storageFile);
                List<BackgroundTransferContentPart> parts = new List<BackgroundTransferContentPart>();
                parts.Add(part);
                UploadOperation operation = await uploader.CreateUploadAsync(new Uri(remoteUri), parts);
                if (completed != null)
                {
                    ResponseInfo response = await GetUploadResponse(operation);
                    completed.Invoke(this, new BackgroundTrasnferEventArgs(response));
                }
            }
        }
        public async void UploadAsync(string remoteUri, string[] files)
        {
            List<BackgroundTransferContentPart> parts = new List<BackgroundTransferContentPart>();
            foreach (var f in files)
            {
                StorageFile storageFile = await StorageFile.GetFileFromPathAsync(f);
                if (storageFile != null)
                {
                    BackgroundTransferContentPart part = new BackgroundTransferContentPart("file", storageFile.Name);
                    part.SetFile(storageFile);
                    parts.Add(part);
                }
            }
            UploadOperation uploadOpration = await uploader.CreateUploadAsync(new Uri("http://localhost:59045/api/upload/"), parts, "form-data");
            if (completed != null)
            {
                ResponseInfo response = await GetUploadResponse(uploadOpration);
                completed.Invoke(this, new BackgroundTrasnferEventArgs(response));
            }
        }
        async Task<ResponseInfo> GetUploadResponse(UploadOperation operation)
        {
            await Task.Delay(1000);
            ResponseInfo ret = new ResponseInfo();
            await operation.StartAsync();
            IInputStream stream = operation.GetResultStreamAt(0);
            StreamReader sr = new StreamReader(stream.AsStreamForRead());
            ret.Response = await sr.ReadToEndAsync();
            var info = operation.GetResponseInformation();
            ret.StatusCode = info.StatusCode;
            ret.Headers = info.Headers;
            ret.RequestUri = info.ActualUri;
            return ret;

        }



        public void SetToastNotification(string xmlString)
        {
            throw new NotImplementedException();
        }
    }
}
