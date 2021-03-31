using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FFMPEG_Demo.Controllers
{
    public class HomeController : Controller
    {
        //https://github.com/cmxl/FFmpeg.NET/blob/master/src/FFmpeg.NET/Engine.cs
        //https://github.com/cmxl/FFmpeg.NET/blob/master/src/FFmpeg.NET/FFmpegProcess.cs
        //https://gist.github.com/AlexMAS/276eed492bc989e13dcce7c78b9e179d

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }

        private void FfmpegProcess_Exited(object sender, EventArgs e)
        {

        }

        public ActionResult Video()
        {
            return View();
        }
    }
}
