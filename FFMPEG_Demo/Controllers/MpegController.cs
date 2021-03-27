using FFMPEG_Demo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace FFMPEG_Demo.Controllers
{
    /* Resource
       https://www.codegrepper.com/code-examples/csharp/C%23+generate+a+new+guid
       
    */
    public class MpegController : ApiController
    {
        [HttpGet]
        public IEnumerable<string> Index()
        {
            return new string[] { "Welcome to Mpeg API" };
        }
        [HttpGet]
        public HttpResponseMessage UniquID()
        {
            Guid g = Guid.NewGuid();
            string uid = Guid.NewGuid().ToString();
            string dt = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            var obj = new { Id = dt + "-" + uid };
            return Request.CreateResponse(HttpStatusCode.OK, obj);
        }
        [HttpPost]
        public HttpResponseMessage CreateFolder(string name = null)
        {
            string _alert = null;
            string path = @"D:\ffmpeg_demo\" + name;
            if (!(Directory.Exists(path)))
            {
                Directory.CreateDirectory(path);
                _alert = "Directory Created Successfully";
            }
            else
            {
                _alert = "Directory Alraedy Existed";
            }
            var obj = new { alert = _alert };
            return Request.CreateResponse(HttpStatusCode.OK, obj);
        }
        [HttpPost]
        public HttpResponseMessage DeleteFolder(string name = null)
        {
            string _alert = null;
            string path = @"D:\ffmpeg_demo\" + name;
            if ((Directory.Exists(path)))
            {
                DeleteDirectory(path);
                _alert = "Directory Deleted Successfully";
            }
            else
            {
                _alert = "Directory Not Existed";
            }
            var obj = new { alert = _alert };
            return Request.CreateResponse(HttpStatusCode.OK, obj);
        }
        [HttpPost]
        public HttpResponseMessage CreateKey(CreateKeyBody createKeyBody)
        {
            string _alert = null;
            if (!String.IsNullOrWhiteSpace(createKeyBody.Id))
            {
                string path = @"D:\ffmpeg_demo\" + createKeyBody.Id;
                if (!(Directory.Exists(path)))
                {
                    CreateFolder(createKeyBody.Id);                    
                }
                //open ssl cmd here
                _alert = "Key File Created Successfully";

            } else
            {
                _alert = "Please provide Content Id";
            }
            
            var obj = new { alert = _alert };
            return Request.CreateResponse(HttpStatusCode.OK, obj);
        }

        #region IgnoreApi
        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        private void DeleteDirectory(string path)
        {
            // Delete all files from the Directory  
            foreach (string filename in Directory.GetFiles(path))
            {
                File.Delete(filename);
            }
            // Check all child Directories and delete files  
            foreach (string subfolder in Directory.GetDirectories(path))
            {
                DeleteDirectory(subfolder);
            }
            Directory.Delete(path);
        }
        #endregion
    }
}
