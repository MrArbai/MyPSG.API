using System.Collections.Generic;
using Dapper.Contrib.Extensions;


namespace MyPSG.API.Models.Auth
{
    [Table("tbl_utl_header_menu")]
    public class MenuHeader : BaseModel
    {
        [ExplicitKey]
        public int Header_id { get; set; }
        public int Menu_id { get; set; }
        public string Header_name { get; set; }
        public int Column_no { get; set; }   
        
    }
}