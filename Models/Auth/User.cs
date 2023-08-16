using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace MyPSG.API.Models.Auth
{
    [Table("tbl_Auth_user")]
    public class User : BaseModel
    {
        
        //internal IEnumerable<RolePrivilege> rolePrivileges;

        [ExplicitKey]
        public string User_guid { get; set; }
        public string User_id { get; set; }
        public string Company_id { get; set; }
        public string Role_id { get; set; }
        public int Employee_id { get; set; }
        public string User_name { get; set; }
        public string Password { get; set; }
        public string Password_key { get; set; }
        
        public short Status_user { get; set; }
        [Write(false)]
        public string Token { get; set; }
        [Write(false)]
        public string Sign_id { get; set; }
        public string No_hp { get; set; }
        public string Telegram_id { get; set; }
        [Write(false)]
        public Role Role { get; set; }
        [Write(false)]
        public List<RolePrivilege> RolePrivileges { get; set; }
        public int Otp { get;  set; }
        public string Bot_token { get; internal set; } = "5104251316:AAFQfc2jG0cm7zjbRLJzg0O__hQT6g1ijuo";
    
    }
}