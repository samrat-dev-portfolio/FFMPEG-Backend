using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FFMPEG_Demo.Models
{
    public class GetContents
    {
        public string contentID { get; set; }
        public string contentTitle { get; set; }
        public string contentFileName { get; set; }
        public string IsConversion { get; set; }
    }
}