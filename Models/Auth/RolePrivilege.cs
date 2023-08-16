using Dapper.Contrib.Extensions;

namespace MyPSG.API.Models.Auth
{
    [Table("tbl_utl_role_privilege")]
    public class RolePrivilege : BaseModel
    {
        [ExplicitKey] 
        public string Role_id { get; set; }
        public int Menu_id { get; set; }
        public int Grant_id { get; set; }
        public bool Is_grant { get; set; }
        public string Nama_menu {get; set;}
        public string Judul_menu {get; set;}

    }
}