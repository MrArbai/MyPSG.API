using Dapper.Contrib.Extensions;
using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace MyPSG.API.Models.Master
{
    [System.ComponentModel.DataAnnotations.Schema.Table("tbl_mst_item_uom")]
    public class ItemUom : BaseModel
    {
        [ExplicitKey]
        [Column("uom_id")]
        public string Uom_Id { get; set; }
        [Column("uom_name")]
        public string Uom_Name { get; set; }
        [Column("revision_no")]
        public short Revision_No { get; set; }
    }
}


