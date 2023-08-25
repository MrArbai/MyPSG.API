using Dapper.Contrib.Extensions;

namespace MyPSG.API.Models.Auth
{
    [Table("tbl_auth_role")]
    public class Role : BaseModel
    {
        [ExplicitKey]
        public string role_id { get; set; }
        public string role_name { get; set; }
        public string company_id { get; set; }        
        public string description { get; set; }       
    }
}

