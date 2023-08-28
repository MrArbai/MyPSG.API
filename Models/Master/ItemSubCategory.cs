using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations.Schema;


namespace MyPSG.API.Models.Master
{
    [System.ComponentModel.DataAnnotations.Schema.Table("tbl_mst_item_subcategory")]
    public class ItemSubCategory : BaseModel
    {
        [ExplicitKey]
        [Column("subcategory_id")]
        public int SubCategory_id { get; set; }
        [Column("subcategory_name")]
        public int SubCategory_name { get; set; }
        [Column("revision_no")]
        public string Revision_no { get; set; }
        [Column("category_id")]
        public string Category_id { get; set; }
    }
}


