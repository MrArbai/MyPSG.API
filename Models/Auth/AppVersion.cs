using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Http;

namespace MyPSG.API.Models.Auth
{
    [Table("tbl_Auth_app_version_info")]
    public class AppVersionInfo : BaseModel
    {
        //ExplicitKey]
        public long id { get; set; }
        public string app_id { get; set; }
        public string last_version { get; set; }
        public string bug_fixed { get; set; }
        public string file_update_name { get; set; }
        
    }

    public class AppVersionInfoXml : AppVersionInfo
    {
        public IFormFile File { get; set; }
        public string Url { get; set; }
        public string UrlInet { get; set; }
        public string FileName { get; set; }
        public string MD5 { get; set; }
        public string LaunchArgs { get; set; }
    }
}

