using System;
using MyPSG.API.Models;
using MyPSG.API.Models.Master;

namespace MyPSG.API.Dto.Master
{
    public class ItemDto : Item
    {
        public ItemSubCategory Sub_Category { get; set; }
        public ItemCategory Category { get; set; }
        public ItemUom UOMID { get; set; }
        public ItemUom PurcheserUOM { get; set; }
    }

    public class ItemParam
    {
        private readonly QueryHelper Qh = new();
        public string Item { get; set; }
        public string SubCategory { get; set; }
        public string Category_ID { get; set; }
        public string Query
        {
            get
            {
                string cond = "";
                if (Category_ID != null)
                {
                    cond = Qh.SetConditionAND(cond, string.Format(@"C.category_id = {0})", Category_ID));
                }
                if (SubCategory != null)
                {
                    cond = Qh.SetConditionAND(cond, string.Format(@"A.subcategory_id = {0}) ", SubCategory));
                }
                if (Item != null)
                {
                    cond = Qh.SetConditionAND(cond, string.Format(@"A.item_id LIKE '%{0}%' OR A.item_name LIKE '%{0}%'", Item));
                }
                var sql = string.Format(@"SELECT A.item_id As id_item,A.*,
                                            CONCAT(A.subcategory_id,A.item_id) As id_subcategory,B.*,
                                            CONCAT(A.subcategory_id,A.item_id,C.category_id) As id_category ,C.*,
                                            CONCAT(A.uom_id,A.item_id) As id_uom,D.*,
                                            CONCAT(A.purchase_uom_id,A.item_id) As id_purchase_uom,E.*
                                            FROM tbl_mst_Item A 
                                            JOIN tbl_mst_item_subcategory B ON A.subcategory_id = B.subcategory_id
                                            JOIN tbl_mst_item_category C ON B.category_id = C.category_id
                                            JOIN tbl_mst_item_uom D ON A.uom_id = D.uom_id
                                            JOIN tbl_mst_item_uom E ON A.purchase_uom_id = E.uom_id {0}",
                                            cond == "" ? "" : " WHERE " + cond);

                return sql;
            }
        }
    }
}