using Dapper.Contrib.Extensions;

namespace MyPSG.API.Models.Auth
{
    [Table("tbl_auth_menu_item")]
    public class MenuItem : BaseModel
    {
        public string item_menu_id { get; set; }
        public string menu_id { get; set; }
        public int? grant_id { get; set; }
        public string keterangan { get; set; }
       

    }
}