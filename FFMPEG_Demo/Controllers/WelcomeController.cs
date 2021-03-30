using Dapper;
using FFMPEG_Demo.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Xml;


namespace FFMPEG_Demo.Controllers
{
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


        #region Test
        [NonAction]
        public string Menu()
        {
            string xmlFiles = listXmlFiles();
            string[] xmlArr = xmlFiles.Split(',');
            foreach (var file in xmlArr)
            {
                if (!string.IsNullOrEmpty(file)) {
                    var fileArr = file.Split('_');
                    string my_class = null;
                    string my_subject = null;
                    string my_chapters = null;
                    if(fileArr.Length >= 3)
                    {
                        my_class = fileArr[1];
                        my_subject = fileArr[2];
                        if(fileArr.Length > 3)
                        {
                            my_subject += fileArr[3];
                        }
                       
                        my_chapters = xml2menu(file);
                    }
                }
            }
            return xmlFiles;
        }
        [NonAction]
        public string listXmlFiles()
        {
            var path = System.Web.Hosting.HostingEnvironment.MapPath("~/XmlMenu");
            string str = "";
            if (Directory.Exists(path))
            {
                DirectoryInfo d = new DirectoryInfo(path);
                FileInfo[] Files = d.GetFiles("*.xml");
                foreach (FileInfo file in Files)
                {
                    str = str + ", " + file.Name;
                }
                string pattern = "[.]xml";
                str = Regex.Replace(str, pattern, "");
            }
            return str;
        }
        [NonAction]
        public string xml2menu(string name)
        {
            string ret = "";
            var pathXmlFile = System.Web.Hosting.HostingEnvironment.MapPath("~/XmlMenu/" + name.Trim() + ".xml");
            if (System.IO.File.Exists(pathXmlFile))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(pathXmlFile);
                string xmlcontents = doc.InnerXml;

                string pattern = "(<[?]xml(.)+[?]>)|(</?Information>)";
                string dataText = Regex.Replace(xmlcontents, pattern, "");

                pattern = "(<Data[\\d]+>)";
                dataText = Regex.Replace(dataText, pattern, "");

                pattern = "(</Data[\\d]+>)";
                dataText = Regex.Replace(dataText, pattern, "~");
                ret = dataText;
            }
            return ret;
        }
        #endregion
    }
}