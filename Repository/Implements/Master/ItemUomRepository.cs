using System;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using MyPSG.API.Repository.Interfaces;
using MyPSG.API.Models.Master;
using MyPSG.API.Repository.Interfaces.Master;

namespace MyPSG.API.Repository.Implements.Master
{
    internal class ItemUomRepository : IItemUomRepository
    {
        private readonly IDapperContext _context;

        public ItemUomRepository(IDapperContext context)
        {
            _context = context;
        }

        public async Task<ItemUom> GetByID(string Uom_id)
        {
            try
            {
                var ItemUom = await Task.Run(() => _context.Db.Get<ItemUom>(Uom_id));
                return ItemUom;
            }
            catch (Exception ex)
            {
                throw new Exception("GetByID" + ex.Message);
            }


        }
        public async Task<ItemUom> Save(ItemUom Uom)
        {
             try
            {
                Uom.Uom_Id = _context.GetGUID();
                await Task.Run(() => _context.Db.InsertAsync(Uom));
                return Uom;             
            }
            catch (Exception ex)
            {
                throw new Exception("Save" + ex.Message);
            }
        }
        public async Task<ItemUom> Update(ItemUom Uom)
        {
             try
            {
                Uom.Revision_No++;
                await Task.Run(() => _context.Db.UpdateAsync(Uom));
                return Uom;
                
            }
            catch (Exception ex)
            {
                throw new Exception("Save" + ex.Message);
            }
        }
        public async Task Delete(ItemUom Uom)
        {
             try
            {
                await Task.Run(() => _context.Db.Delete(Uom));
            }
            catch (Exception ex)
            {
                throw new Exception("Save" + ex.Message);
            }
        }
        
    }
}