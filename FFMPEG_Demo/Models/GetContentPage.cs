using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FFMPEG_Demo.Models
{
    public class GetContentPage
    {
        public string pageindex { get; set; }
        public string limit { get; set; }
        public string orderby { get; set; }
        public string desc { get; set; }
        public string contentID { get; set; }
        public string contentFileName { get; set; }
        public string contentTitle { get; set; }
    }
}