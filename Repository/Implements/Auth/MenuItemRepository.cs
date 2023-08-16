using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using MyPSG.API.Models.Auth;
using MyPSG.API.Repository.Interfaces;
using MyPSG.API.Repository.Interfaces.Auth;

namespace MyPSG.API.Repository.Implements.Auth
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private IDapperContext _context;
        private IDbTransaction _transaction;
        // private ILog _log;

        public MenuItemRepository(IDapperContext context)
        {
            _context = context;
            // _log = log;
        }

        public Task Delete(MenuItem obj)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MenuItem>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MenuItem>> GetByMenuId(int id)
        {
            return await _context.db.QueryAsync<MenuItem>("SELECT * FROM tbl_utl_item_menu WHERE menu_id = @id", new {id = id});
        }

        public Task<MenuItem> Save(MenuItem obj)
        {
            throw new NotImplementedException();
        }

        public Task Update(MenuItem obj)
        {
            throw new NotImplementedException();
        }
    }
}