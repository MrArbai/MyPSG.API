using System.Threading.Tasks;
using MyPSG.API.Models.Master;

namespace MyPSG.API.Repository.Interfaces.Master
{
    public interface IItemUomRepository
    {
        Task<ItemUom> GetByID(string ItemID);
        Task<ItemUom> Save(ItemUom Uom);
        Task<ItemUom> Update(ItemUom Uom);
        Task Delete(ItemUom Item);
    }
}