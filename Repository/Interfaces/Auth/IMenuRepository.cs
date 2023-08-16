using System.Collections.Generic;
using System.Threading.Tasks;
using MyPSG.API.Models.Auth;

// ======= Author : Ozzy ========
// ======= Created : 29/03/2021 =

namespace MyPSG.API.Repository.Interfaces.Auth
{
    public interface IMenuRepository
    {
        Task<IEnumerable<Menu>> GetAllMenu();
        Task<IEnumerable<MenuHeader>> GetAllHeader();
    }
}