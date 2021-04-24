using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FFMPEG_Demo.Models
{
    public class GetChapterPage
    {
        public string pageindex { get; set; }
        public string limit { get; set; }
        public string orderby { get; set; }
        public string desc { get; set; }

        public string id { get; set; }
        public string chapterName { get; set; }
        public string subjectName { get; set; }
        public string className { get; set; }
        public string contentID { get; set; }
    }
}