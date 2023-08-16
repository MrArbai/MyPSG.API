using System.Collections.Generic;
using System.Threading.Tasks;
using MyPSG.API.Models.Auth;

namespace MyPSG.API.Repository.Interfaces.Auth
{
    public interface IRolePrivilegeRepository
    {
        Task<IEnumerable<RolePrivilege>> GetAll();
        Task<RolePrivilege> GetRoleByMenuNameAndGrant(string role_id, string nama_menu, int grant_id);
        Task<RolePrivilege> GetRoleByMenuIDAndGrant(string role_id, string menu_id, int grant_id); 
        Task<IEnumerable<RolePrivilege>> GetRoleByID(string role_id);
        Task<RolePrivilege> SaveRole(RolePrivilege dt);
    }
}