using Dapper.Contrib.Extensions;

namespace MyPSG.API.Models.Auth
{
    [Table("tbl_utl_role_privilege")]
    public class RolePrivilege
    {
        [ExplicitKey] 
        public string role_id { get; set; }
        public int menu_id { get; set; }
        public int grant_id { get; set; }
        public bool is_grant { get; set; }
        public string nama_menu {get; set;}
        public string judul_menu {get; set;}

    }
}
