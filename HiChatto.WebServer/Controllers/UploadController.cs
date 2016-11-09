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
        public async Task<HttpResponseMessage> PostFormData()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            string timeFile = DateTime.Now.ToFileTime().ToString();
            string relativePath = "upload/" + timeFile;
            string root = HttpContext.Current.Server.MapPath("~/" + relativePath);
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            var provider = new MultipartFormDataStreamProvider(root);
            string path = "192.168.137.1:8888";
            var task =await Request.Content.ReadAsMultipartAsync(provider);
            StringBuilder builder = new StringBuilder();
            foreach (var item in task.FileData)
            {
                string filename = item.Headers.ContentDisposition.FileName.Split('\"')[1];
                string filepath = root + "\\" + filename;
                builder.Append(string.Format("http://{0}/{1}/{2}\n", path, relativePath, filename));
                File.Move(item.LocalFileName, filepath);
            }
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(builder.ToString()) };
        }

    }
}
