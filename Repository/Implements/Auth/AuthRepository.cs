using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using MyPSG.API.Models.Auth;
using MyPSG.API.Repository.Interfaces;
using MyPSG.API.Repository.Interfaces.Auth;

namespace MyPSG.API.Repository.Implements.Auth
{
    internal class AuthRepository : IAuthRepository
    {
        private IDapperContext _context;
        // private ILog _log;

        public AuthRepository(IDapperContext context)
        {
            _context = context;
            // _log = log;
        }
        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Db.GetAllAsync<User>();
        }
        public async Task<bool> Changepassword(string username, string passwordold, string passwordnew)
        {
            var user = await Task.Run(() => _context.Db.Get<User>(username));
            var psold = BCrypt.Net.BCrypt.HashPassword(passwordold);
            if (user == null || user.password != psold)
                return false;

            var psnew = BCrypt.Net.BCrypt.HashPassword(passwordnew);
            user.password = psnew;
            await _context.Db.UpdateAsync(user);

            return true;
        }

        public async Task<bool> Changepassword(string username, string passwordnew)
        {
            var user = await Task.Run(() => _context.Db.Get<User>(username));
            
            var psnew = BCrypt.Net.BCrypt.HashPassword(passwordnew);
            user.password = psnew;
            await _context.Db.UpdateAsync(user);

            return true;
        }
        public async Task Login(UserLoginInfo userLoginDto)
        {
            await _context.Db.InsertAsync(userLoginDto);
        }

        public async Task<User> Login(string userID, string password)
        {
            try
            {
                var user = await Task.Run(() => _context.Db.Get<User>(userID)) ?? throw new Exception("User Tidak terdaftar !!!");
                if (!BCrypt.Net.BCrypt.Verify(password, user.password))
                    throw new Exception("password Salah, Silahkan Periksa password Anda !!!");  

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<User> Register(User user)
        {
            string pass = BCrypt.Net.BCrypt.HashPassword(user.password);
            user.password = pass;
            await Save(user);

            return user;
        }
        public async Task<User> Update(User obj)
        {
            var user = await Task.Run(() => _context.Db.Get<User>(obj.user_id));

            obj.password =  user.password;

            await _context.Db.UpdateAsync(obj);
            return obj;
        }
        public async Task<User> Save(User obj)
        {
            await _context.Db.InsertAsync(obj);
            return null;
        }


        public async Task<bool> UserExists(string username)
        {
            if (await _context.Db.ExecuteScalarAsync<bool>("Select Count(1) From tbl_Auth_user where " +
                           "user_id = @userId", new { userId = username }))
                return true;

            return false;
        }

        public async Task<Role> GetRoleByID(string id)
        {
            return await Task.Run(() => _context.Db.Get<Role>(id));
        }

        public async Task<IEnumerable<RolePrivilege>> GetAll(string role_id)
        {
            string sql;

            sql = string.Format(@"SELECT a.role_id, a.grant_id, a.is_grant, b.menu_id, b.nama_menu, b.is_active FROM tbl_Auth_role_privilege as a INNER JOIN tbl_Auth_menu as b ON a.menu_id = b.menu_id WHERE a.role_id='{0}'", role_id);

            return await _context.Db.QueryAsync<RolePrivilege>(sql);
        }
        public async Task<AppVersionInfo> GetAppVersions()
        {
            try
            {
                var version = await _context.Db.QueryFirstOrDefaultAsync<AppVersionInfo>("SELECT * FROM tbl_auth_app_version_info ORDER BY id DESC LIMIT 1") ?? throw new Exception("Data Tersedia");
                return version;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}