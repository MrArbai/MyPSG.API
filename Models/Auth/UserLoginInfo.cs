using System;
using Dapper.Contrib.Extensions;

namespace MyPSG.API.Models.Auth
{
    [Table("tbl_Auth_login_info")]
    public class UserLoginInfo
    {
        [ExplicitKey]
        public string login_guid { get; set; }
        public DateTime? login_date { get; set; }
        public string login_id { get; set; }
        public DateTime?  logout_date { get; set; }
        public string computer_name { get; set; }
        public string login_type { get; set; }
        public string app_version { get; set; }
        public string ip_address { get; set; }
    }
}