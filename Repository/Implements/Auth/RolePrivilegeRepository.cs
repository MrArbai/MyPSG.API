using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using MyPSG.API.Models.Auth;
using MyPSG.API.Repository.Interfaces;
using MyPSG.API.Repository.Interfaces.Auth;

namespace MyPSG.API.Repository.Implements.Auth
{
    internal class RolePrivilegeRepository : IRolePrivilegeRepository
    {
        private IDapperContext _context;
        private IDbTransaction _transaction;

        public RolePrivilegeRepository(IDapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RolePrivilege>> GetAll()
        {
            return await _context.Db.QueryAsync<RolePrivilege>("SELECT a.role_id, a.grant_id, a.is_grant, b.menu_id, b.nama_menu, b.is_active FROM tbl_Auth_role_privilege as a INNER JOIN tbl_Auth_menu as b ON a.menu_id = b.menu_id");

        }

        public async Task<RolePrivilege> GetRoleByMenuNameAndGrant(string role_id, string nama_menu, int grant_id)
        {
            return await _context.Db.QueryFirstOrDefaultAsync<RolePrivilege>("SELECT a.role_id, a.grant_id, a.is_grant, b.menu_id, b.nama_menu, b.judul_menu, b.is_active FROM tbl_Auth_role_privilege as a INNER JOIN tbl_Auth_menu as b ON a.menu_id = b.menu_id where a.role_id = @l1 and b.nama_menu=@l2 and a.grant_id=@l3", 
            new { l1 = role_id, l2 = nama_menu, l3 = grant_id });
        }

        public async Task<RolePrivilege> GetRoleByMenuIDAndGrant(string role_id, string menu_id, int grant_id)
        {
            return await _context.Db.QueryFirstOrDefaultAsync<RolePrivilege>("SELECT a.role_id, a.grant_id, a.is_grant, b.menu_id, b.nama_menu, b.judul_menu, b.is_active FROM tbl_Auth_role_privilege as a INNER JOIN tbl_Auth_menu as b ON a.menu_id = b.menu_id where a.role_id = @l1 and b.menu_id=@l2 and a.grant_id=@l3", 
            new { l1 = role_id, l2 = menu_id, l3 = grant_id });
        }

        public async Task<IEnumerable<RolePrivilege>> GetRoleByID(string role_id)
        {
            string sql;

            sql = string.Format(@"SELECT a.role_id, a.grant_id, a.is_grant, b.menu_id, b.judul_menu, b.is_active FROM tbl_Auth_role_privilege as a INNER JOIN tbl_Auth_menu as b ON a.menu_id = b.menu_id WHERE a.role_id='{0}'", role_id);

            return await _context.Db.QueryAsync<RolePrivilege>(sql);

        }

        public async Task<RolePrivilege> SaveRole(RolePrivilege dt)
        {
            _context.BeginTransaction();
            _transaction = _context.Transaction;

            try
            {
                var sql = @"CALL public.sp_utl_role_privilege_save (@p_role_id::character varying, @p_menu_id::integer, @p_grant_id::integer, @p_is_grant::boolean)";
                var p = new DynamicParameters();
                p.Add("@p_role_id", dt.role_id);
                p.Add("@p_menu_id", dt.menu_id);
                p.Add("@p_grant_id", dt.grant_id);
                p.Add("@p_is_grant", dt.is_grant);

                await _context.Db.ExecuteAsync(sql, p, transaction:_transaction);


                var hasil = await _context.Db.QueryFirstOrDefaultAsync<RolePrivilege>("SELECT * FROM tbl_Auth_role_privilege WHERE role_id = @l1 AND menu_id = @l2 AND grant_id = @l3", 
                new { l1 = dt.role_id, l2 = dt.menu_id, l3 = dt.grant_id });
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