using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations.Schema;


namespace MyPSG.API.Models.Master
{
    [System.ComponentModel.DataAnnotations.Schema.Table("tbl_mst_item_uom")]
    public class ItemUom : BaseModel
    {
        [ExplicitKey]
        [Column("uom_id")]
        public int Uom_Id { get; set; }
        [Column("uom_name")]
        public int Uom_Name { get; set; }
        [Column("revision_no")]
        public string Revision_No { get; set; }
    }
}


