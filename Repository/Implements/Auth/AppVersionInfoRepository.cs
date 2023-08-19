using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using MyPSG.API.Models.Auth;
using MyPSG.API.Repository.Interfaces;
using MyPSG.API.Repository.Interfaces.Auth;

namespace MyPSG.API.Repository.Implements.Auth
{
    public class AppVersionInfoRepository : IAppVersionInfoRepository
    {
        private readonly IDapperContext _context;

        public AppVersionInfoRepository(IDapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AppVersionInfo>> GetAll()
        {
            return await _context.Db.GetAllAsync<AppVersionInfo>();
        }

        public async Task<AppVersionInfo> GetLastAppVersion()
        {
            return await _context.Db.QueryFirstOrDefaultAsync<AppVersionInfo>("SELECT TOP 1 * FROM tbl_Auth_app_version_info ORDER BY id DESC ");
        }

        public async Task<AppVersionInfo> Save(AppVersionInfo obj)
        {
            AppVersionInfo dt = new();
            await _context.Db.InsertAsync(obj);
            return dt;
        }

        public async Task<AppVersionInfo> Update(AppVersionInfo obj)
        {
             await _context.Db.QueryAsync("UPDATE tbl_Auth_app_version_info SET bug_fixed = @BugFixed WHERE id = @ID",
                new {BugFixed = obj.bug_fixed, ID = obj.id }
            );

            return obj;
        }

        public async Task UpdateVersion(long id, string AppVersion)
        {
            await _context.Db.QueryAsync("UPDATE tbl_Auth_app_version_info SET last_version = @AppVersion WHERE id = @id", new { AppVersion, id });
        }
    }
}