using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyPSG.API.Models.Auth
{
    [System.ComponentModel.DataAnnotations.Schema.Table("tbl_auth_user")]
    public class User : BaseModel
    {
        [ExplicitKey]
        [Column("user_id")]
        public string User_id { get; set; }
        [Column("company_id")]
        public string Company_id { get; set; }
        [Column("role_id")]
        public string Role_id { get; set; }
        [Column("employee_id")]
        public int Employee_id { get; set; }
        [Column("user_name")]
        public string User_name { get; set; }
        [Column("password")]
        public string Password { get; set; }
        [Column("user_guid")]
        public string User_guid { get; set; }
        [Column("status_user")]        
        public short Status_user { get; set; }
        [Write(false)]
        [Column("token")]
        public string Token { get; set; }
        [Write(false)]
        [Column("sign_id")]
        public string Sign_id { get; set; }
        [Column("no_hp")]
        public string No_hp { get; set; }
    
    }
}