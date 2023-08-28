using System;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using MyPSG.API.Repository.Interfaces;
using MyPSG.API.Repository.Interfaces.Master;
using MyPSG.API.Models.Master;

namespace MyPSG.API.Repository.Implements.Master
{
    internal class ItemCategoryRepository : IItemCategoryRepository
    {
        private readonly IDapperContext _context;

        public ItemCategoryRepository(IDapperContext context)
        {
            _context = context;
        }

        public async Task<ItemCategory> GetByID(int Category_id)
        {
            try
            {
                var ItemCategory = await Task.Run(() => _context.Db.Get<ItemCategory>(Category_id));
                return ItemCategory;
            }
            catch (Exception ex)
            {
                throw new Exception("GetByID", ex);
            }


        }
        public async Task Save(ItemCategory Item)
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
        public async Task Update(ItemCategory Item)
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
        public async Task Delete(ItemCategory Item)
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
        
    }
}