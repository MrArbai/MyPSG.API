using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyPSG.API.Models.Auth
{
    [Table("tbl_auth_login_info")]
    public class UserLoginInfo
    {
        [Dapper.Contrib.Extensions.ExplicitKey]
        [Column("login_guid")]
        public string login_guid { get; set; }
        [Column("login_date")]
        public DateTime? login_date { get; set; }
        [Column("login_id")]
        public string login_id { get; set; }
        [Column("logout_date")]
        public DateTime?  logout_date { get; set; }
        [Column("computer_name")]
        public string computer_name { get; set; }
        [Column("login_type")]
        public string login_type { get; set; }
        [Column("app_version")]
        public string app_version { get; set; }
        [Column("ip_address")]
        public string ip_address { get; set; }
    }
}