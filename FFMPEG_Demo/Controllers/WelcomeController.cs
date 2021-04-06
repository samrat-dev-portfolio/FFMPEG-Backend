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

        public ActionResult Proceed(string my_class = null)
        {
            getClass();
            return View();
        }

        private void getClass()
        {
            string FFMpegCon = ConfigurationManager.ConnectionStrings["FFMpeg"].ConnectionString;
            SqlConnection con = new SqlConnection(FFMpegCon);
            string sql = @"SELECT * FROM [dbo].[tblClass]";
            List<GetClassNames> my_class = con.Query<GetClassNames>(sql).ToList<GetClassNames>();
            ViewData["my_class"] = my_class;

            //string sql = @"INSERT INTO tblPDFstore([filename],[filedata])
            //VALUES (@filename,@filedata); SELECT CAST(SCOPE_IDENTITY() as int); ";
            //var id = con.Query<int>(sql,
            //    new
            //    {
            //        @filename = safeFileName,
            //        @filedata = json,
            //    }).Single();
        }
    }
}