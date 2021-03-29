using FFmpeg.NET;
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

        private static Task<bool> WaitForExitAsync(Process process, int timeout)
        {
            return Task.Run(() => process.WaitForExit(timeout));
        }
        public struct ProcessResult
        {
            public bool Completed;
            public int? ExitCode;
            public string Output;
        }
        public async Task<MediaFile> GetThumbnailAsync()
        {
            string apppath = "C:/FFmpeg/bin/";
            string ffmpegPath = apppath + "ffmpeg.exe";
            string pathToVideoFile = Path.Combine(Server.MapPath("~/UploadedFiles"), "after_the_rain.mp4");
            string pathToImageFileOut = Path.Combine(Server.MapPath("~/UploadedFiles"), "after_the_rain_1.jpg");

            var inputFile = new MediaFile(pathToVideoFile);
            var outputFile = new MediaFile(pathToImageFileOut);
            var ffmpeg = new Engine(ffmpegPath);

            var options = new ConversionOptions { Seek = TimeSpan.FromSeconds(15) };
            return await ffmpeg.GetThumbnailAsync(inputFile, outputFile, options);
        }
        public async Task<MetaData> GetMetadataAsync()
        {
            string apppath = "C:/FFmpeg/bin/";
            string ffmpegPath = apppath + "ffmpeg.exe";
            string pathToVideoFile = Path.Combine(Server.MapPath("~/UploadedFiles"), "after_the_rain.mp4");

            var inputFile = new MediaFile(pathToVideoFile);
            var ffmpeg = new Engine(ffmpegPath);

            var options = new ConversionOptions { Seek = TimeSpan.FromSeconds(15) };
            return await ffmpeg.GetMetaDataAsync(inputFile);
        }

        public ActionResult Video()
        {
            //  public async Task<ActionResult> Video() {}
            //var data = await GetMetadataAsync();
            //var data = await Trim();

            return View();
        }
    }
}
