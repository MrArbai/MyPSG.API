
using MyPSG.API.Repository.Interfaces.Auth;

namespace MyPSG.API.Repository.Interfaces.Master
{
    public interface IUnitOfWorkMaster
    {
        IItemRepository ItemRepository { get; }
        IItemUomRepository ItemUomRepository {get;}
    }
}