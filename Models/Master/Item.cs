using Dapper.Contrib.Extensions;



namespace MyPSG.API.Models.Master
{
    [Table("tbl_Mst_Item")]
    public class Item : BaseModel
    {
        [ExplicitKey]
        public string itemID { get; set; }
        public int revisionNo { get; set; }
        public string itemName { get; set; }
        public string itemDesc { get; set; }
        public int purchaseUOMID { get; set; }
        public decimal conversionUOM { get; set; }  
        public int uomid { get; set; }     
        public int subCategoryID { get; set; }    
        public string imagepath { get; set; }      
        
    }
}
