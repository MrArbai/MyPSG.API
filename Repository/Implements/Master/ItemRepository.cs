using System;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using MyPSG.API.Repository.Interfaces;
using MyPSG.API.Repository.Interfaces.Master;
using MyPSG.API.Models.Master;
using System.Collections.Generic;
using MyPSG.API.Dto.Master;
using Dapper;

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
                throw new Exception("GetByID" + ex.Message);
            }


        }
        public async Task<IEnumerable<Item>> GetAll()
        {
            try
            {
                var Item = await Task.Run(() => _context.Db.GetAll<Item>());
                return Item;
            }
            catch (Exception ex)
            {
                throw new Exception("GetAll" + ex.Message);
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
                throw new Exception("Save" + ex.Message);
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
                throw new Exception("Save" + ex.Message);
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
                throw new Exception("Save" + ex.Message);
            }
        }
        public async Task<IEnumerable<ItemDto>> GetItem(ItemParam param)
        {
            // List<dynamic> queryResult = new();
            var queryResult = await Task.Run(() => _context.Db.QueryAsync<List<dynamic>>(
                param.Query));

            List<ItemDto> items = new();

            // foreach (var item in queryResult)
            // {
            //     ItemDto ItemDto = new();
            //     {
            //         ItemDto.ItemID = item.ToString();
            //     }
            //     items.Add(ItemDto);
            // }

            
            return items;
        }
    }
}