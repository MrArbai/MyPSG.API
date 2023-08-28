using System;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using MyPSG.API.Repository.Interfaces;
using MyPSG.API.Repository.Interfaces.Master;
using MyPSG.API.Models.Master;

namespace MyPSG.API.Repository.Implements.Master
{
    internal class ItemRepository : IItemRepository
    {
        private readonly IDapperContext _context;

        public ItemRepository(IDapperContext context)
        {
            _context = context;
        }

        public async Task<Item> GetByID(string ItemID)
        {
            try
            {
                var Item = await Task.Run(() => _context.Db.Get<Item>(ItemID));
                return Item;
            }
            catch (Exception ex)
            {
                throw new Exception("GetByID", ex);
            }


        }
        public async Task Save(Item Item)
        {
             try
            {
                await Task.Run(() => _context.Db.InsertAsync(Item));              
            }
            catch (Exception ex)
            {
                throw new Exception("Save", ex);
            }
        }
        public async Task Update(Item Item)
        {
             try
            {
                await Task.Run(() => _context.Db.UpdateAsync(Item));
                
            }
            catch (Exception ex)
            {
                throw new Exception("Save", ex);
            }
        }
        public async Task Delete(Item Item)
        {
             try
            {
                await Task.Run(() => _context.Db.Delete(Item));
                
            }
            catch (Exception ex)
            {
                throw new Exception("Save", ex);
            }
        }
        // public async Task<IEnumerable<ItemDto>> GetItem(ItemParam param)
        // {
        //     var Item = new Dictionary<string, ItemDto>();
        //     var Category = new Dictionary<string, ItemCategory>();
        //     var SubCategory = new Dictionary<string, ItemSubCategory>();
        //     var Uom = new Dictionary<string, ItemUom>();
        //     var PurcUom = new Dictionary<string, ItemUom>();

        //     await Task.Run(() => _context.Db.QueryAsync<ItemDto>(
        //         param.Query,
        //         new[]
        //         {
        //             typeof(ItemDto),
        //             typeof(ItemCategory),
        //             typeof(ItemSubCategory),
        //             typeof(ItemUom),
        //             typeof(ItemUom)

        //         }, obj =>
        //     { 
        //         ItemDto item = obj[0] as ItemDto;
        //         ItemCategory category = obj[1] as ItemCategory;
        //         ItemSubCategory subcategory = obj[2] as ItemSubCategory;
        //         ItemUom uom = obj[3] as ItemUom;
        //         ItemUom purchuom = obj[4] as ItemUom;

        //         if (!Item.TryGetValue(item.ItemID, out ItemDto iitem))
        //         {
        //             iitem = item;
        //             iitem.Category = category;
        //             iitem.Sub_Category = subcategory;
        //             iitem.UOMID = uom;
        //             iitem.PurcheserUOM = purchuom;
        //         }
        //         if (!Category.TryGetValue(category.Category_id.ToString(), out ItemCategory ccategory))
        //         {

        //         }
        //     },splitOn: "id_item,id_subcategory,id_category,id_uom,id_purchase_uom"));
        //     return Item.Values;
        // }
    }
}