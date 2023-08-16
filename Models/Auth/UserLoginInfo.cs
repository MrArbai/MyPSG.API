using System;
using Dapper.Contrib.Extensions;

namespace MyPSG.API.Models.Auth
{
    [Table("tbl_utl_login_info")]
    public class UserLoginInfo
    {
        [ExplicitKey]
        public string Login_guid { get; set; }
        public DateTime? Login_date { get; set; }
        public string Login_id { get; set; }
        public DateTime? Logout_date { get; set; }
        public string Computer_name { get; set; }
        public string Login_type { get; set; }
        public string App_version { get; set; }
        public string Ip_address { get; set; }
    }
}