using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyPSG.API.Models.Auth
{
    [Table("tbl_Auth_login_info")]
    public class UserLoginInfo
    {
        [Dapper.Contrib.Extensions.ExplicitKey]
        [Column("login_guid")]
        public string Login_guid { get; set; }
        [Column("login_date")]
        public DateTime? Login_date { get; set; }
        [Column("login_id")]
        public string Login_id { get; set; }
        [Column("logout_date")]
        public DateTime?  Logout_date { get; set; }
        [Column("computer_name")]
        public string Computer_name { get; set; }
        [Column("login_type")]
        public string Login_type { get; set; }
        [Column("app_version")]
        public string App_version { get; set; }
        [Column("ip_address")]
        public string Ip_address { get; set; }
    }
}