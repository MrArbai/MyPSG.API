using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using MyPSG.API.Dto.Auth;
using MyPSG.API.Models.Auth;
using MyPSG.API.Repository.Interfaces;
using MyPSG.API.Repository.Interfaces.Auth;

namespace MyPSG.API.Repository.Implements.Auth
{
    internal class RoleRepository : IRoleRepository
    {
        private IDapperContext _context;
        // private ILog _log;

        public RoleRepository(IDapperContext context)
        {
            _context = context;
            // _log = log;
        }

        public async Task<IEnumerable<Role>> GetAll()
        {
            return await _context.db.QueryAsync<Role>("SELECT * FROM tbl_utl_role order by role_name");

        }

        public async Task<Role> Save(Role obj)
        {
            await _context.db.InsertAsync(obj);
            return null;
        }

        public async Task<Role> Update(Role obj)
        {
            await _context.db.UpdateAsync(obj);
            return null;
        }
        
        public async Task<Role> Delete(Role obj)
        {
            await _context.db.DeleteAsync(obj);
            return null;
        }

        public async Task<Role> GetRoleByMenuNameAndGrant(string role_id, string nama_menu, int grant_id)
        {
            return await _context.db.QueryFirstOrDefaultAsync<Role>("SELECT a.role_id, a.grant_id, a.is_grant, b.menu_id, b.nama_menu, b.is_active FROM tbl_utl_role_privilege as a INNER JOIN tbl_utl_menu as b ON a.menu_id = b.menu_id where a.role_id = @l1 and b.nama_menu=@l2 and a.grant_id=@l3", 
            new { l1 = role_id, l2 = nama_menu, l3 = grant_id });
        }

        public async Task<Role> GetRoleByMenuIDAndGrant(string role_id, string menu_id, int grant_id)
        {
            return await _context.db.QueryFirstOrDefaultAsync<Role>("SELECT a.role_id, a.grant_id, a.is_grant, b.menu_id, b.nama_menu, b.is_active FROM tbl_utl_role_privilege as a INNER JOIN tbl_utl_menu as b ON a.menu_id = b.menu_id where a.role_id = @l1 and b.menu_id=@l2 and a.grant_id=@l3", 
            new { l1 = role_id, l2 = menu_id, l3 = grant_id });
        }

        public async Task<IEnumerable<Role>> GetRoleByID(string role_id)
        {
            string sql;

            sql = string.Format(@"SELECT a.role_id, a.grant_id, a.is_grant, b.menu_id, b.nama_menu, b.is_active FROM tbl_utl_role_privilege as a INNER JOIN tbl_utl_menu as b ON a.menu_id = b.menu_id WHERE a.role_id='{0}'", role_id);

            return await _context.db.QueryAsync<Role>(sql);

        }
        public async Task<IEnumerable<Role>> GetByParam(RoleDto param)
        {
            string sql;

            sql = string.Format(@"SELECT * FROM tbl_utl_role  order by role_name");

            return await _context.db.QueryAsync<Role>(sql);

        }

    }
}