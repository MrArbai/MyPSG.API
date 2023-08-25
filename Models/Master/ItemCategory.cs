using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations.Schema;


namespace MyPSG.API.Models.Master
{
    [System.ComponentModel.DataAnnotations.Schema.Table("tbl_mst_item_category")]
    public class ItemCategory : BaseModel
    {
        [ExplicitKey]
        [Column("category_id")]
        public int Category_id { get; set; }
        [Column("revision_no")]
        public int Revision_no { get; set; }
        [Column("category_name")]
        public string Category_name { get; set; }
        [Column("acc_id")]
        public string Acc_id { get; set; }
    }
}


