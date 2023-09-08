using System.Collections.Generic;
using System.Threading.Tasks;
using MyPSG.API.Dto.Master;
using MyPSG.API.Models.Master;

namespace MyPSG.API.Repository.Interfaces.Master
{
    public interface IItemRepository
    {
        Task<Item> GetByID(string ItemID);
        Task Save(Item Item);
        Task Update(Item Item);
        Task Delete(Item Item);
        Task<IEnumerable<Item>> GetAll();
        Task<IEnumerable<ItemDto>> GetItem(ItemParam param);
    }
}