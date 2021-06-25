using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FFMPEG_Demo.Models
{
    public class LicenseGenerateKey
    {
        public string LicenceAppId { get; set; }
        public string LicenceKey { get; set; }
        public string creationDate { get; set; }
    }
    public class LicenseKeyGenPageQuery
    {
        public string pageindex { get; set; }
        public string limit { get; set; }
        public string orderby { get; set; }
        public string desc { get; set; }

        public string appId { get; set; }
        public string serialKey { get; set; }
        public string creationDate { get; set; }
    }
    public class LicenseKeyGenPage
    {
        public string appId { get; set; }
        public string serialKey { get; set; }
        public string creationDate { get; set; }
        public string deviceId { get; set; }
        public string machineName { get; set; }
        public string clientName { get; set; }
        public string description { get; set; }
    }
}