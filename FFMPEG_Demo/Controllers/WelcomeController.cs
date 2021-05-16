using Dapper;
using FFMPEG_Demo.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http.Cors;
using System.Web.Mvc;
using System.Xml;


namespace FFMPEG_Demo.Controllers
{
    [AllowCrossSite]
    public class WelcomeController : Controller
    {
        // GET: Welcome
        public ActionResult Index()
        {
            return View();
        }
    }
}