using System.Collections.Generic;
using Dapper.Contrib.Extensions;


namespace MyPSG.API.Models.Auth
{
    [Table("tbl_Auth_header_menu")]
    public class MenuHeader : BaseModel
    {
       [ExplicitKey]
        public int header_id { get; set; }
        public int menu_id { get; set; }
        public string header_name { get; set; }
        public int column_no { get; set; }    
        
    }
}