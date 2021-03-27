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

        public void ManuallyFFMPEG()
        {
            string apppath = "C:/FFmpeg/bin/";
            string pathToVideoFile = Path.Combine(Server.MapPath("~/UploadedFiles"), "after_the_rain.mp4");
            string pathToVideoFileOut = Path.Combine(Server.MapPath("~/UploadedFiles"), "after_the_rain_1.mp4");
            string fileargs = "-i" + " " + pathToVideoFile + " " + pathToVideoFileOut; // is extension change "-i nesha.mp4 -ss 00:01:00 -t 00:00:20 xxc.mp4" 
            string fileargs_1 = "-i" + "" + pathToVideoFile + " " + "-ss 00:01:00" + " " + "-t 00:00:20" + " " + pathToVideoFileOut; //  is trim from 1-min length 20-sec
            Process p = new Process();
            p.StartInfo.FileName = apppath + "ffmpeg.exe";
            p.StartInfo.Arguments = fileargs_1;
            p.StartInfo.UseShellExecute = true;
            p.StartInfo.CreateNoWindow = false;
            p.StartInfo.RedirectStandardOutput = false;
            p.Start();
            p.Close();
        }

        public async Task<ProcessResult> Trim()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            var result = new ProcessResult();
            string apppath = "C:/FFmpeg/bin/";
            string ffmpegPath = apppath + "ffmpeg.exe";
            string pathToVideoFile = Path.Combine(Server.MapPath("~/UploadedFiles"), "after_the_rain.mp4");
            string pathToVideoFileOut = Path.Combine(Server.MapPath("~/UploadedFiles"), "after_the_rain_1.mp4");
            string pathToVideoFileOut2 = Path.Combine(Server.MapPath("~/UploadedFiles"), "after_the_rain_1.webm");
            string fileargs = "-i" + " " + pathToVideoFile + " " + "-ss 00:01:00" + " " + "-t 00:00:20" + " " + pathToVideoFileOut;
            string fileargs2 = "-i" + " " + pathToVideoFile + " " + pathToVideoFileOut2;

            int timeout = 10000;

            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                Arguments = fileargs2,
                FileName = ffmpegPath,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            Process ffmpegProcess = new Process() { StartInfo = startInfo };

            var outputBuilder = new StringBuilder();
            var outputCloseEvent = new TaskCompletionSource<bool>();
            ffmpegProcess.OutputDataReceived += (s, e) =>
            {
                if (e.Data == null)
                {
                    outputCloseEvent.SetResult(true);
                }
                else
                {
                    outputBuilder.AppendLine(e.Data);
                }
            };

            var errorBuilder = new StringBuilder();
            var errorCloseEvent = new TaskCompletionSource<bool>();
            ffmpegProcess.ErrorDataReceived += (s, e) =>
            {
                // The error stream has been closed i.e. the process has terminated
                if (e.Data == null)
                {
                    errorCloseEvent.SetResult(true);
                }
                else
                {
                    errorBuilder.AppendLine(e.Data);
                }
            };

            ffmpegProcess.Exited += FfmpegProcess_Exited;

            bool isStarted;
            try
            {
                isStarted = ffmpegProcess.Start();

            }
            catch (Exception error)
            {
                // Usually it occurs when an executable file is not found or is not executable

                result.Completed = true;
                result.ExitCode = -1;
                result.Output = error.Message;

                isStarted = false;
            }

            if (isStarted)
            {
                ffmpegProcess.BeginOutputReadLine();
                ffmpegProcess.BeginErrorReadLine();

                var waitForExit = WaitForExitAsync(ffmpegProcess, timeout);
                var processTask = Task.WhenAll(waitForExit, outputCloseEvent.Task, errorCloseEvent.Task);
                if (await Task.WhenAny(Task.Delay(timeout), processTask) == processTask && waitForExit.Result)
                {
                    result.Completed = true;
                    result.ExitCode = ffmpegProcess.ExitCode;

                    // Adds process output if it was completed with error
                    if (ffmpegProcess.ExitCode != 0)
                    {
                        result.Output = $"{outputBuilder}{errorBuilder}";
                    }
                }
                else
                {
                    try
                    {
                        // Kill hung process
                        ffmpegProcess.Kill();
                    }
                    catch
                    {
                    }
                }
            }
            return result;
            //ffmpegProcess.WaitForExit();

        }

        private void FfmpegProcess_Exited(object sender, EventArgs e)
        {
            int i = 0;
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

        public async Task<ActionResult> Video()
        {
            //var data = await GetMetadataAsync();
            var data = await Trim();

            return View();
        }
    }
}
