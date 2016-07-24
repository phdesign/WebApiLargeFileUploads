using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebApiLargeFileUploads.Controllers
{
    public class DefaultController : ApiController
    {
        [HttpPost]
        public async Task<HttpResponseMessage> Upload1(HttpPostedFileBase file)
        {
            if (file == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "No file supplied");
            var url = "";
            var response = await PostMultipartStreamAsync(url, file.FileName, file.InputStream);
            // TODO: Do something with response
            return Request.CreateResponse(HttpStatusCode.OK, "Success");
        }

        private async Task<string> PostMultipartStreamAsync(string url, string fileName, Stream fileStream)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromHours(2);
                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StreamContent(fileStream), "file", fileName);
                    using (var message = await httpClient.PostAsync(url, content).ConfigureAwait(false))
                    {
                        var response = await message.Content.ReadAsStringAsync().ConfigureAwait(false);
                        return response;
                    }
                }
            }
        }
    }
}
