using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using MyPSG.API.Models.Auth;
using MyPSG.API.Repository.Interfaces;
using MyPSG.API.Repository.Interfaces.Auth;


namespace Opium.Api.Repository.Implements.Utl
{
    internal class MenuRepository : IMenuRepository
    {
        private IDapperContext _context;

        public MenuRepository(IDapperContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Menu>> GetAllMenu()
        {            
           return await Task.Run(() =>  _context.Db.GetAllAsync<Menu>());
        }
        public async Task<IEnumerable<MenuHeader>> GetAllHeader()
        {
            return await _context.Db.GetAllAsync<MenuHeader>();
        }
    }
}