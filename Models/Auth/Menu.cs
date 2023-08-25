using System.ComponentModel.DataAnnotations.Schema;


namespace MyPSG.API.Models.Auth
{
    [Table("tbl_auth_menu")]
    public class Menu : BaseModel
    {
        [Dapper.Contrib.Extensions.ExplicitKey]
        [Column("menu_id")]
        public string Menu_id { get; set; }
        [Column("nama_menu")]
        public string Nama_menu { get; set; }
        [Column("judul_menu")]
        public string Judul_menu { get; set; }
        [Column("parent_id")]
        public string Parent_id { get; set; }
        [Column("order_number")]
        
        public int? Order_number { get; set; }
        [Column("nama_form")]
        public string Nama_form { get; set; }  
        [Column("is_enabled")]
        public bool Is_enabled { get; set; } 
        [Column("is_header")]    
        public string Is_header { get; set; }  
        [Column("header_id")]  
        public string Header_id { get; set; }  
        
    }
}