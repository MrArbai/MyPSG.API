using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations.Schema;


namespace MyPSG.API.Models.Master
{
    [System.ComponentModel.DataAnnotations.Schema.Table("tbl_mst_Item")]
    public class Item : BaseModel
    {
        [ExplicitKey]
        [Column("item_id")]
        public string ItemID { get; set; }
        [Column("revision_no")]
        public int RevisionNo { get; set; }
        [Column("item_name")]
        public string ItemName { get; set; }
        [Column("itemDesc")]
        public string ItemDesc { get; set; }
        [Column("purchase_uom_id")]
        public int PurchaseUOMID { get; set; }
        [Column("conversion_uom")]
        public decimal ConversionUOM { get; set; } 
        [Column("uom_id")] 
        public int Uomid { get; set; }  
        [Column("subcategory_id")]   
        public int SubCategoryID { get; set; }  
        [Column("image_path")]  
        public string Imagepath { get; set; }      
        
    }
}


