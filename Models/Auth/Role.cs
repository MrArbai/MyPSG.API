using Dapper.Contrib.Extensions;

namespace MyPSG.API.Models.Auth
{
    [Table("tbl_utl_role")]
    public class Role : BaseModel
    {
        [ExplicitKey]
        public string Role_id { get; set; }
        public string Role_name { get; set; }
        public string Company_id { get; set; }        
        public string Description { get; set; }      
    }
}