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
        private IDapperContext context;


        public MenuRepository(IDapperContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Menu>> GetAllMenu()
        {
            //return await _context.db.GetAllAsync<Menu>();
            var sql = @"SELECT * FROM tbl_utl_menu";
            
           return await _context.db.QueryAsync<Menu>(sql);
        }
        public async Task<IEnumerable<MenuHeader>> GetAllHeader()
        {
            return await _context.db.GetAllAsync<MenuHeader>();
        }
    }
}