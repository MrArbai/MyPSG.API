using System.Collections.Generic;
using System.Threading.Tasks;
using MyPSG.API.Models.Auth;

namespace MyPSG.API.Repository.Interfaces.Auth
{
    public interface IAuthRepository
    {
        
        Task<IEnumerable<User>> GetAll();
        Task<User> Register(User user);
        Task<User> Update(User user);
        Task<bool> UserExists(string username);
        Task Login(UserLoginInfo userLoginDto);
        Task<User> Login(string username, string password);
        Task<bool> Changepassword(string username, string passwordold, string passwordnew);
        Task<bool> Changepassword(string username, string passwordnew);   
        Task<Role> GetRoleByID(string id);  
        Task<IEnumerable<RolePrivilege>> GetAll(string role_id);
        Task<AppVersionInfo> GetAppVersions();
    }
}