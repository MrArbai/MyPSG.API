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
    public class AppVersionInfoRepository : IAppVersionInfoRepository
    {
        private IDapperContext _context;
        private IDbTransaction _transaction;

        public AppVersionInfoRepository(IDapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AppVersionInfo>> GetAll()
        {
            return await _context.db.GetAllAsync<AppVersionInfo>();
        }

        public async Task<AppVersionInfo> GetLastAppVersion()
        {
            return await _context.db.QueryFirstOrDefaultAsync<AppVersionInfo>("SELECT TOP 1 * FROM tbl_utl_app_version_info ORDER BY id DESC ");
        }

        public async Task<AppVersionInfo> Save(AppVersionInfo obj)
        {
            AppVersionInfo dt = new AppVersionInfo();
            await _context.db.InsertAsync(obj);
            return dt;
        }

        public async Task<AppVersionInfo> Update(AppVersionInfo obj)
        {
             await _context.db.QueryAsync("UPDATE tbl_utl_app_version_info SET bug_fixed = @BugFixed WHERE id = @ID",
                new {BugFixed = obj.bug_fixed, ID = obj.id }
            );

            return obj;
        }

        public async Task UpdateVersion(long id, string AppVersion)
        {
            await _context.db.QueryAsync("UPDATE tbl_utl_app_version_info SET last_version = @AppVersion WHERE id = @id", new {AppVersion = AppVersion, id = id});
        }
    }
}