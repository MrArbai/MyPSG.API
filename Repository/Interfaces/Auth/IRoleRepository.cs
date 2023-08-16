using System.Collections.Generic;
using System.Threading.Tasks;
using MyPSG.API.Dto.Auth;
using MyPSG.API.Models.Auth;

namespace MyPSG.API.Repository.Interfaces.Auth
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAll();
        Task<Role> GetRoleByMenuNameAndGrant(string role_id, string nama_menu, int grant_id);
        Task<Role> GetRoleByMenuIDAndGrant(string role_id, string menu_id, int grant_id);
        Task<IEnumerable<Role>> GetRoleByID(string role_id);
        Task<IEnumerable<Role>> GetByParam(RoleDto param);
        Task<Role> Save (Role param);
        Task<Role> Update (Role param);
        Task<Role> Delete (Role param);
    }
}