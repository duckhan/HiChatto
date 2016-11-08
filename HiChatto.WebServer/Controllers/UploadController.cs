using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
namespace HiChatto.Webserver.Controllers
{
    public class UploadController : ApiController
    {
        public Task<HttpResponseMessage> PostFormData()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return new Task<HttpResponseMessage>(() => { return new HttpResponseMessage(HttpStatusCode.InternalServerError); });
            }
            string root = HttpContext.Current.Server.MapPath("~/upload");
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            var provider = new MultipartFormDataStreamProvider(root);
            string path = Request.RequestUri.Authority;
            var task = Request.Content.ReadAsMultipartAsync(provider).
                ContinueWith((a) =>
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (var item in provider.FileData)
                    {
                        string[] str = item.Headers.ContentDisposition.FileName.Split('\"')[1].Split('.');
                        string filename = "";
                        if (str.Length > 1)
                        {
                            filename = string.Join(".", str, 0, str.Length - 1) + "_" + DateTime.Now.ToFileTime() + "." + str[str.Length - 1];
                        }
                        else
                        {
                            filename = str[0] + "_" + DateTime.Now.ToFileTime();
                        }
                        string filepath = root + "\\" + filename;
                        builder.AppendLine(string.Format("{0}/upload/{1}", path, filename));
                        File.Move(item.LocalFileName, filepath);
                    }
                    return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(builder.ToString()) };
                });
            return task;
        }

    }
}
