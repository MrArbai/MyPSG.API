using Dapper.Contrib.Extensions;



namespace MyPSG.API.Models.Auth
{
    [Table("tbl_utl_menu")]
    public class Menu : BaseModel
    {
        [ExplicitKey]
        public string Menu_id { get; set; }
        public string Nama_menu { get; set; }
        public string Judul_menu { get; set; }
        public string Parent_id { get; set; }
        public int? Order_number { get; set; }
        public string Nama_form { get; set; }  
        public bool Is_enabled { get; set; }     
        public string Is_header { get; set; }    
        public string Header_id { get; set; }      
        
    }
}