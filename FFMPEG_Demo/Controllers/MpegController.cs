﻿using FFMPEG_Demo.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Description;

namespace FFMPEG_Demo.Controllers
{
    /* Resource
       https://www.codegrepper.com/code-examples/csharp/C%23+generate+a+new+guid
       https://www.codeproject.com/Articles/25983/How-to-Execute-a-Command-in-C
       https://hlsbook.net/category/ffmpeg/
       https://stackoverflow.com/questions/4291912/process-start-how-to-get-the-output

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
            string base_content_storage = ConfigurationManager.AppSettings["base_content_storage"];
            string path = base_content_storage + name;
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
            string base_content_storage = ConfigurationManager.AppSettings["base_content_storage"];
            string path = base_content_storage + name;
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
                string base_content_storage = ConfigurationManager.AppSettings["base_content_storage"];
                string path = base_content_storage + createKeyBody.Id;
                if (!(Directory.Exists(path)))
                {
                    CreateFolder(createKeyBody.Id);
                }
                //open ssl cmd here
                RunSSL(createKeyBody.Id);
                _alert = "Key File Created Successfully";
            }
            else
            {
                _alert = "Please provide Content Id";
            }

            var obj = new { alert = _alert };
            return Request.CreateResponse(HttpStatusCode.OK, obj);
        }
        [HttpGet]
        public HttpResponseMessage Keyinfo(string id = null)
        {
            //var path = System.Web.HttpContext.Current.Server.MapPath("~/player_content/" + id);
            string base_content_storage = ConfigurationManager.AppSettings["base_content_storage"];
            var fullpath = Path.Combine(base_content_storage, id, "enc.key");
            string _alert = null;
            if (Directory.Exists(base_content_storage + id))
            {
                if (File.Exists(fullpath))
                {
                    var dataBytes = File.ReadAllBytes(fullpath);
                    var dataStream = new MemoryStream(dataBytes);
                    HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
                    httpResponseMessage.Content = new StreamContent(dataStream);
                    httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    httpResponseMessage.Content.Headers.ContentDisposition.FileName = "enc.key";
                    httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                    return httpResponseMessage;
                }
                else
                {
                    _alert = "file not found";
                }
            }
            else
            {
                _alert = "file not found";
            }
            var obj = new { alert = _alert };
            return Request.CreateResponse(HttpStatusCode.OK, obj);
        }
        [HttpGet]
        public HttpResponseMessage M3u8info(string id = null)
        {
            //var path = System.Web.HttpContext.Current.Server.MapPath("~/player_content/" + id);
            string base_content_storage = ConfigurationManager.AppSettings["base_content_storage"];
            string filename = "index.m3u8";
            var fullpath = Path.Combine(base_content_storage, id, filename);
            string _alert = null;
            if (Directory.Exists(base_content_storage + id))
            {
                if (File.Exists(fullpath))
                {
                    var dataBytes = File.ReadAllBytes(fullpath);
                    var dataStream = new MemoryStream(dataBytes);
                    dataStream = M3u8DataManipulation(dataStream, id);
                    HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
                    httpResponseMessage.Content = new StreamContent(dataStream);
                    httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    httpResponseMessage.Content.Headers.ContentDisposition.FileName = filename;
                    httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                    return httpResponseMessage;
                }
                else
                {
                    _alert = "file not found";
                }
            }
            else
            {
                _alert = "file not found";
            }
            var obj = new { alert = _alert };
            return Request.CreateResponse(HttpStatusCode.OK, obj);
        }
        [HttpGet]
        public HttpResponseMessage Tsinfo(string id = null, string filename = null)
        {
            //var path = System.Web.HttpContext.Current.Server.MapPath("~/player_content/" + id);
            string base_content_storage = ConfigurationManager.AppSettings["base_content_storage"];
            //string filename = "index.m3u8";
            var fullpath = Path.Combine(base_content_storage, id, filename + ".ts");
            string _alert = null;
            if (Directory.Exists(base_content_storage + id))
            {
                if (File.Exists(fullpath))
                {
                    var dataBytes = File.ReadAllBytes(fullpath);
                    var dataStream = new MemoryStream(dataBytes);
                    HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
                    httpResponseMessage.Content = new StreamContent(dataStream);
                    httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    httpResponseMessage.Content.Headers.ContentDisposition.FileName = filename + ".ts";
                    httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                    return httpResponseMessage;
                }
                else
                {
                    _alert = "file not found";
                }
            }
            else
            {
                _alert = "file not found";
            }
            var obj = new { alert = _alert };
            return Request.CreateResponse(HttpStatusCode.OK, obj);
        }
        [HttpGet]
        public HttpResponseMessage MediaInfo(string id = null)
        {
            string info = RunFFM_Info(id);
            string duration = getInfoPattern(info, "duration");
            string fps = getInfoPattern(info, "fps");
            string frame = "0";
            if (!string.IsNullOrEmpty(duration) && !string.IsNullOrEmpty(fps))
            {
                double _frame = 0;
                string[] ds = duration.Split(':');
                if (ds.Length == 3)
                {
                    double hour = Convert.ToDouble(ds[0]) * 60 * 60;
                    double min = Convert.ToDouble(ds[1]) * 60;
                    double sec = Convert.ToDouble(ds[2]);
                    _frame = (sec + min + hour) * Convert.ToDouble(fps);
                }
                frame = Convert.ToString(_frame);
            }

            var obj = new { duration, fps, frame };
            return Request.CreateResponse(HttpStatusCode.OK, obj);
        }
        [HttpGet]
        public HttpResponseMessage ConversionProgressInfo(string id = null)
        {
            string base_content_storage = ConfigurationManager.AppSettings["base_content_storage"];
            var fullpath = Path.Combine(base_content_storage, id, "block.txt");
            string currentFrame = null;
            string status = null;
            if (File.Exists(fullpath))
            {

                string data = File.ReadAllText(fullpath);
                currentFrame = getInfoPattern(data, "progress");
                status = getInfoPattern(data, "progress_status");
            }
            var obj = new { status, currentFrame };
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

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        private void RunSSL(string name)
        {
            string base_content_storage = ConfigurationManager.AppSettings["base_content_storage"];
            string path = base_content_storage + name;
            string command = "openssl rand 16 > enc.key";

            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/c " + command;
            p.StartInfo.WorkingDirectory = path;
            p.StartInfo.UseShellExecute = false;
            // Do not create the black window.
            p.StartInfo.CreateNoWindow = true;
            // This means that it will be redirected to the Process.StandardOutput StreamReader.
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();
            // Get the output into a string
            string result = p.StandardOutput.ReadToEnd();
            //p.Close();
            CreateKeyInfo(name);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        private void CreateKeyInfo(string name)
        {
            string base_content_storage = ConfigurationManager.AppSettings["base_content_storage"];
            string path = base_content_storage + name + @"\enc.keyinfo";
            // Check if file already exists. If yes, delete it. 
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            // Create a new file     
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine("/enc.key");
                sw.WriteLine("enc.key");
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        private MemoryStream M3u8DataManipulation(MemoryStream _stream, string id)
        {
            StreamReader dataReader = new StreamReader(_stream);
            string dataText = dataReader.ReadToEnd();
            // data Manipulation
            //string id = "20210327-181439-b9093c54-0c6e-4ae6-bdac-c70f0e9f7a95";
            //string base_api_Keyinfo = @"http://localhost:50017/api/mpeg/Keyinfo/";
            //string base_api_Tsinfo = @"http://localhost:50017/api/mpeg/Tsinfo/";

            string base_api_Keyinfo = ConfigurationManager.AppSettings["base_api_Keyinfo"];
            string base_api_Tsinfo = ConfigurationManager.AppSettings["base_api_Tsinfo"];

            string pattern_URI = "URI=\"(.)+\"";  // http://localhost:50017/api/mpeg/Keyinfo/20210327-181439-b9093c54-0c6e-4ae6-bdac-c70f0e9f7a95
            string newURI = "URI=\"" + base_api_Keyinfo + id + "\"";

            string pattern_segment = "(segmentNo)";
            string newSegment = base_api_Tsinfo + id + "/segmentNo";  // http://localhost:50017/api/mpeg/Tsinfo/{id}/segmentNo0

            //RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Multiline;
            dataText = Regex.Replace(dataText, pattern_URI, newURI);
            dataText = Regex.Replace(dataText, pattern_segment, newSegment);
            dataText = Regex.Replace(dataText, "(.ts)", "");

            var manipulationStream = new MemoryStream();
            var writer = new StreamWriter(manipulationStream);
            writer.Write(dataText);
            writer.Flush();
            manipulationStream.Position = 0;
            return manipulationStream;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        private string RunFFM_Info(string name)
        {
            string _alert = null;
            string base_content_storage = ConfigurationManager.AppSettings["base_content_storage"];
            string mp4_filename = "Aawara_Shaam.mp4";
            string path = base_content_storage + name;
            string command = "ffmpeg -i " + mp4_filename;

            if (File.Exists(path + "/" + mp4_filename))
            {
                Process p = new Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.Arguments = "/c " + command;
                p.StartInfo.WorkingDirectory = path;
                p.StartInfo.UseShellExecute = false;
                // Do not create the black window.
                p.StartInfo.CreateNoWindow = true;
                // This means that it will be redirected to the Process.StandardOutput StreamReader.
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.Start();
                // Get the output into a string
                string result = p.StandardOutput.ReadToEnd();
                string err = p.StandardError.ReadToEnd();
                result += "\n" + err;
                p.WaitForExit();
                //p.Close();
                _alert = result;
            }
            return _alert;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        private string getInfoPattern(string str, string types)
        {
            string pattern = "";
            if ("duration" == types)
            {
                pattern = @"(?<=Duration: )[\w\:.]+";
            }
            else if ("fps" == types)
            {
                pattern = @"[\d]+(?= fps)";
            }
            else if ("progress" == types)
            {
                pattern = @"(?<=frame=)[\d]+";
            }
            else if ("progress_status" == types)
            {
                pattern = @"(?<=progress=)[\w]+";
            }
            RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Multiline;
            MatchCollection match = Regex.Matches(str, pattern, options);
            if (match.Count > 0)
            {
                if ("progress" == types || "progress_status" == types)
                {
                    str = match[match.Count - 1].Value.ToString().Trim();
                }
                else
                {
                    str = match[0].Value.ToString().Trim();
                }
            }
            return str;
        }

        #endregion
    }
}
