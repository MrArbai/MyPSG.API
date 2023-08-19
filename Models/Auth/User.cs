using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace MyPSG.API.Models.Auth
{
    [Table("tbl_Auth_user")]
    public class User : BaseModel
    {
        
        //internal IEnumerable<RolePrivilege> rolePrivileges;

        
        
        [ExplicitKey]
        public string user_id { get; set; }
        public string company_id { get; set; }
        public string role_id { get; set; }
        public int employee_id { get; set; }
        public string user_name { get; set; }
        public string password { get; set; }
        public string user_guid { get; set; }
        
        public short status_user { get; set; }
        [Write(false)]
        public string token { get; set; }
        [Write(false)]
        public string sign_id { get; set; }
        public string no_hp { get; set; }
    
    }
}