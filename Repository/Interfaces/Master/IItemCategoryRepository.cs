using System.Threading.Tasks;
using MyPSG.API.Models.Master;

namespace MyPSG.API.Repository.Interfaces.Master
{
    public interface IItemCategoryRepository
    {
        Task<ItemCategory> GetByID(int ItemID);
        Task Save(ItemCategory Item);
        Task Update(ItemCategory Item);
        Task Delete(ItemCategory Item);
    }
}