using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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
            return await _context.db.GetAllAsync<User>();
        }
        public async Task<bool> ChangePassword(string username, string passwordold, string passwordnew)
        {
            var user = await _context.db.QueryFirstOrDefaultAsync<User>("Select * FROM tbl_utl_user WHERE user_id = @userid", new { userid = username });
            var psold = CreatePasswordHash(passwordold, user.Password_key);
            if (user == null || user.Password != psold)
                return false;

            var psnew = CreatePasswordHash(passwordnew, user.Password_key);
            await _context.db.QueryAsync("Update tbl_utl_user SET password = @psnew, last_updated_by = @users, last_updated_date = @tgl Where user_id = @userid",
                    new { psnew = psnew, users = username, tgl = DateTime.Now,  userid = username});

            return true;
        }

        public async Task<bool> ChangePassword(string username, string passwordnew)
        {
            var user = await _context.db.QueryFirstOrDefaultAsync<User>("Select * FROM tbl_utl_user WHERE user_id = @userid", new { userid = username });
            
            var psnew = CreatePasswordHash(passwordnew, user.Password_key);
            await _context.db.QueryAsync("Update tbl_utl_user SET password = @psnew, last_updated_by = @users, last_updated_date = @tgl Where user_id = @userid",
                    new { psnew = psnew, users = username, tgl = DateTime.Now,  userid = username});

            return true;
        }
        public async Task Login(UserLoginInfo userLoginDto)
        {
            await _context.db.InsertAsync(userLoginDto);
        }

        public async Task<User> Login(string username, string password)
        {
            try
            {
                var user = await _context.db.QueryFirstOrDefaultAsync<User>("Select * FROM tbl_utl_user WHERE user_id = @userid", new { userid = username });

                if (user == null)
                    throw new Exception("User Tidak terdaftar !!!");

                if (!VerifyPassword(password, user.Password, user.Password_key))
                    throw new Exception("Password Salah, Silahkan Periksa Password Anda !!!");

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool VerifyPassword(string pass, string password, string passwordKey)
        {
            var passUser = CreatePasswordHash(pass, passwordKey);

            if (passUser != password)
                return false;

            return true;
        }

        private string CreatePasswordHash(string plainText, string key)
        {
            if (key.Length > 0)
                plainText += key;

            var bs = System.Text.Encoding.UTF8.GetBytes(plainText);
            bs = MD5.HashData(bs);

            var s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            string password = s.ToString();
            return password;
        }


        public async Task<User> Register(User user)
        {
            string pass = CreatePasswordHash(user.Password, user.Password_key);
            user.Password = pass;
            await this.Save(user);

            return user;
        }
        public async Task<User> Update(User obj)
        {
            var user = await _context.db.QueryFirstOrDefaultAsync<User>("Select * FROM tbl_utl_user WHERE user_id = @userid", new { userid = obj.User_id });
            var psold = CreatePasswordHash(obj.Password, user.Password_key);
            obj.Password =  user.Password;
            obj.Password_key = user.Password_key;

            await _context.db.UpdateAsync<User>(obj);
            return obj;
        }
        public async Task<User> Save(User obj)
        {
            await _context.db.InsertAsync<User>(obj);
            // await _context.db.QueryAsync("INSERT INTO tbl_utl_user VALUES ()",
            //         new { psnew = psnew, users = obj.user_id, tgl = DateTime.Now,  userid = username});

            return null;
        }


        public async Task<bool> UserExists(string username)
        {
            if (await _context.db.ExecuteScalarAsync<bool>("Select Count(1) From tbl_utl_user where " +
                           "user_id = @userId", new { userId = username }))
                return true;

            return false;
        }

        public async Task<Role> GetRoleByID(string id)
        {
            return await _context.db.QueryFirstOrDefaultAsync<Role>("SELECT * FROM tbl_utl_role WHERE role_id = @id", new {id = id});
        }

        public async Task<IEnumerable<RolePrivilege>> GetAll(string role_id)
        {
            string sql;

            sql = string.Format(@"SELECT a.role_id, a.grant_id, a.is_grant, b.menu_id, b.nama_menu, b.is_active FROM tbl_utl_role_privilege as a INNER JOIN tbl_utl_menu as b ON a.menu_id = b.menu_id WHERE a.role_id='{0}'", role_id);

            return await _context.db.QueryAsync<RolePrivilege>(sql);
        }
        public async Task<AppVersionInfo> GetAppVersions()
        {
            //return await _context.db.GetAllAsync<AppVersionDto>();
            try
            {
                var version = await _context.db.QueryFirstOrDefaultAsync<AppVersionInfo>("select TOP 1 * from tbl_utl_app_version_info ORDER BY id DESC ");

                if (version == null)
                    throw new Exception("Data Tersedia");
                return version;
            }
            catch (Exception ex)
            {
                // TODO
                // _log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }

    }
}