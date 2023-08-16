using System.Collections.Generic;
using System.Threading.Tasks;
using MyPSG.API.Models.Auth;

namespace MyPSG.API.Repository.Interfaces.Auth
{
    public interface IAppVersionInfoRepository 
    {
        Task<AppVersionInfo> Save(AppVersionInfo obj);
        Task<AppVersionInfo> Update(AppVersionInfo obj);
        Task<AppVersionInfo> GetLastAppVersion();
        Task UpdateVersion(long id, string AppVersion);
        Task<IEnumerable<AppVersionInfo>> GetAll();
    }
}