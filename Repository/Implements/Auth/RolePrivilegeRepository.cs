using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using MyPSG.API.Models.Auth;
using MyPSG.API.Repository.Interfaces;
using MyPSG.API.Repository.Interfaces.Auth;

namespace MyPSG.API.Repository.Implements.Auth
{
    internal class RolePrivilegeRepository : IRolePrivilegeRepository
    {
        private IDapperContext _context;
        private IDbTransaction _transaction;
        // private ILog _log;

        public RolePrivilegeRepository(IDapperContext context)
        {
            _context = context;
            // _log = log;
        }

        public async Task<IEnumerable<RolePrivilege>> GetAll()
        {
            return await _context.db.QueryAsync<RolePrivilege>("SELECT a.role_id, a.grant_id, a.is_grant, b.menu_id, b.nama_menu, b.is_active FROM tbl_utl_role_privilege as a INNER JOIN tbl_utl_menu as b ON a.menu_id = b.menu_id");

        }

        public async Task<User> Save(User obj)
        {
            await _context.db.InsertAsync(obj);
            return null;
        }

        public async Task<RolePrivilege> GetRoleByMenuNameAndGrant(string role_id, string nama_menu, int grant_id)
        {
            return await _context.db.QueryFirstOrDefaultAsync<RolePrivilege>("SELECT a.role_id, a.grant_id, a.is_grant, b.menu_id, b.nama_menu, b.judul_menu, b.is_active FROM tbl_utl_role_privilege as a INNER JOIN tbl_utl_menu as b ON a.menu_id = b.menu_id where a.role_id = @l1 and b.nama_menu=@l2 and a.grant_id=@l3", 
            new { l1 = role_id, l2 = nama_menu, l3 = grant_id });
        }

        public async Task<RolePrivilege> GetRoleByMenuIDAndGrant(string role_id, string menu_id, int grant_id)
        {
            return await _context.db.QueryFirstOrDefaultAsync<RolePrivilege>("SELECT a.role_id, a.grant_id, a.is_grant, b.menu_id, b.nama_menu, b.judul_menu, b.is_active FROM tbl_utl_role_privilege as a INNER JOIN tbl_utl_menu as b ON a.menu_id = b.menu_id where a.role_id = @l1 and b.menu_id=@l2 and a.grant_id=@l3", 
            new { l1 = role_id, l2 = menu_id, l3 = grant_id });
        }

        public async Task<IEnumerable<RolePrivilege>> GetRoleByID(string role_id)
        {
            string sql;

            sql = string.Format(@"SELECT a.role_id, a.grant_id, a.is_grant, b.menu_id, b.judul_menu, b.is_active FROM tbl_utl_role_privilege as a INNER JOIN tbl_utl_menu as b ON a.menu_id = b.menu_id WHERE a.role_id='{0}'", role_id);

            return await _context.db.QueryAsync<RolePrivilege>(sql);

        }

        public async Task<RolePrivilege> SaveRole(RolePrivilege dt)
        {
            _context.BeginTransaction();
            _transaction = _context.transaction;

            try
            {
                var sql = @"CALL public.sp_utl_role_privilege_save (@p_role_id::character varying, @p_menu_id::integer, @p_grant_id::integer, @p_is_grant::boolean)";
                var p = new DynamicParameters();
                p.Add("@p_role_id", dt.Role_id);
                p.Add("@p_menu_id", dt.Menu_id);
                p.Add("@p_grant_id", dt.Grant_id);
                p.Add("@p_is_grant", dt.Is_grant);

                await _context.db.ExecuteAsync(sql, p, transaction:_transaction);


                var hasil = await _context.db.QueryFirstOrDefaultAsync<RolePrivilege>("SELECT * FROM tbl_utl_role_privilege WHERE role_id = @l1 AND menu_id = @l2 AND grant_id = @l3", 
                new { l1 = dt.Role_id, l2 = dt.Menu_id, l3 = dt.Grant_id });
                _context.Commit();

                return hasil;  
                                             
            }
            catch (System.Exception e)
            {
                _context.Rollback();
                throw new System.Exception(e.Message);
            }

        }
    }
}