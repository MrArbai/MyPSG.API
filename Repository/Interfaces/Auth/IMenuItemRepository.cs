using System.Collections.Generic;
using System.Threading.Tasks;
using MyPSG.API.Models.Auth;
using MyPSG.API.Repositorys.Interfaces;

namespace MyPSG.API.Repository.Interfaces.Auth
{
    public interface IMenuItemRepository : IBaseRepository<MenuItem>
    {
        Task<IEnumerable<MenuItem>> GetByMenuId (int id);
    }
}