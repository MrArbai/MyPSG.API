using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyPSG.API.Dto.Auth;
using MyPSG.API.Models;
using MyPSG.API.Models.Auth;
using MyPSG.API.Repository.Implements;
using MyPSG.API.Repository.Interfaces;

namespace MyPSG.API.Controllers.Auth
{
    [Route("mypsgapi/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(AuthController));
        private IUnitOfWork _uow;
        private IDapperContext _context;
        private readonly IConfiguration _config;
        private IHttpContextAccessor _httpContext;

        public AuthController(IConfiguration config)
        {
            _config = config;
            _httpContext = new HttpContextAccessor();
        }

        [HttpPost("Save"),Authorize]
        public async Task<IActionResult> Register(UserRegisterDto userDto)
        {
            string userby = _httpContext.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            string company_id = _httpContext.HttpContext.User.FindFirst(ClaimTypes.GroupSid).Value;

            userDto.User_id = userDto.User_id.ToLower();

            try
            {
                bool flag;
                using (_context = new DapperContext())
                {
                    _uow = new UnitOfWork(_context);
                    flag = await _uow.AuthRepository.UserExists(userDto.User_id);
                }
                if (flag)
                {
                    var st = StTrans.SetSt(400, 0, "User Sudah Di Buat");
                    return Ok(new { Status = st });
                }

                var usercreate = new User();
                var st2 = StTrans.SetSt(200, 0, "User Berhasil Di Buat");
                using (_context = new DapperContext())
                {
                    _uow = new UnitOfWork(_context);
                    User user = new()
                    {
                        User_id = userDto.User_id,
                        Role_id = userDto.Role_id,
                        Employee_id = userDto.Employee_id,
                        User_name = userDto.User_name,
                        Password = userDto.Password,
                        User_guid = _context.GetGUID(),
                        Is_active = true,
                        Company_id = company_id,
                        Status_user = userDto.Status_user,
                        Created_by = userby,
                        Created_date = DateTime.Now,
                        Computer_name = userDto.Computer,
                        Computer_date = DateTime.Now
                    };
                    usercreate = await _uow.AuthRepository.Register(user);
                }

                return Ok(new { Status = st2, Results = usercreate });

            }
            catch (Exception e)
            {
                var st = StTrans.SetSt(400, 0, e.Message);
                return Ok(new { Status = st });
            }
        }

        [HttpPost("Update"),Authorize]
        public async Task<IActionResult> Update(User user)
        {
            try
            {
                var usercreate = new User();
                var st2 = StTrans.SetSt(200, 0, "User berhasil di perbaharui");
                using (_context = new DapperContext())
                {
                    _uow = new UnitOfWork(_context);
                    usercreate = await _uow.AuthRepository.Update(user);
                }

                return Ok(new { Status = st2, Results = usercreate });

            }
            catch (Exception e)
            {
                var st = StTrans.SetSt(400, 0, e.Message);
                return Ok(new { Status = st });
            }
        }

        [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                IEnumerable<User> hasil;
                using (IDapperContext _context = new DapperContext())
                {
                    var _uow = new UnitOfWork(_context);
                    hasil = await _uow.AuthRepository.GetAll();
                }

                var st2 = StTrans.SetSt(200, 0, "Data di temukan");
                return Ok(new { Status = st2, Results = hasil });
            }
            catch (Exception e)
            {
                var st = StTrans.SetSt(400, 0, e.Message);
                return Ok(new { Status = st });
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto userDto)
        {
            try
            {
                var dt = new User();

                using (_context = new DapperContext())
                {
                    _uow = new UnitOfWork(_context);
                    dt = await _uow.AuthRepository.Login(userDto.User_id.ToLower(), userDto.Password);
                    var ds = await _uow.AuthRepository.GetAppVersions();
                    var dtl = new UserDto()
                    {
                        User_id = dt.User_id,
                        User_guid = dt.User_guid,
                        User_name = dt.User_name,
                        Status_user = dt.Status_user,
                        Company_id = dt.Company_id,
                        Role_id = dt.Role_id,
                        Employee_id = dt.Employee_id,
                        Password = dt.Password,
                        Sign_id = dt.Sign_id,
                        No_hp = dt.No_hp,
                        Token = dt.Token,

                    };
                    UserLoginInfo users = new()
                    {
                        login_guid = _context.GetGUID(),
                        login_date = DateTime.Now,
                        login_id = userDto.User_id.ToLower(),
                        computer_name = userDto.Computer,
                        login_type = "O",
                        app_version = ds.last_version,
                        ip_address = Request.HttpContext.Connection.RemoteIpAddress.ToString()
                    };

                    await _uow.AuthRepository.Login(users);
                    dt.Sign_id = users.login_id;

                    if (dt == null)
                        return Unauthorized();

                    dtl.Role = await _uow.AuthRepository.GetRoleByID(dt.Role_id);
                    dt.Token = GenerateJwtToken(dt, ds.last_version);

                    IEnumerable<RolePrivilege> hasil;
                    hasil = await _uow.RolePrivilegeRepository.GetAll();

                    dtl.RolePrivileges = hasil.ToList();
                }

                var st = StTrans.SetSt(200, 0, "User Berhasil Login");
                return Ok(new { Status = st, Results = dt });
            }
            catch (Exception e)
            {
                var st = StTrans.SetSt(400, 0, e.Message);
                _log.Error(e.Message);
                return Ok(new { Status = st });
            }
        }

        [HttpPost("ChangePassword"),Authorize]
        public async Task<IActionResult> ChangePassword(UserChangePasswordDto cur)
        {
            string userby = _httpContext.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            try
            {

                using (_context = new DapperContext())
                {
                    _uow = new UnitOfWork(_context);
                    await _uow.AuthRepository.ChangePassword(cur.User_id, cur.PasswordOld, cur.PasswordNew);
                }

                LogicalThreadContext.Properties["NewValue"] = Logs.ToJson(cur);
                LogicalThreadContext.Properties["User"] = userby;
                _log.Info("Succes Update");

                var st = StTrans.SetSt(200, 0, "Succes");
                return Ok(new { Status = st, Results = cur });
            }
            catch (Exception e)
            {
                var st = StTrans.SetSt(400, 0, e.Message);

                LogicalThreadContext.Properties["NewValue"] = Logs.ToJson(cur);
                LogicalThreadContext.Properties["User"] = userby;
                _log.Error("Error : ", e);
                return Ok(new { Status = st });
            }
        }

        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(UserChangePasswordDto cur)
        {
            string userby = _httpContext.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            try
            {

                using (_context = new DapperContext())
                {
                    string pass = "123";
                    _uow = new UnitOfWork(_context);
                    await _uow.AuthRepository.ChangePassword(cur.User_id, pass);
                }

                LogicalThreadContext.Properties["NewValue"] = Logs.ToJson(cur);
                LogicalThreadContext.Properties["User"] = userby;
                _log.Info("Succes Update");

                var st = StTrans.SetSt(200, 0, "Succes");
                return Ok(new { Status = st, Results = cur });
            }
            catch (Exception e)
            {
                var st = StTrans.SetSt(400, 0, e.Message);

                LogicalThreadContext.Properties["NewValue"] = Logs.ToJson(cur);
                LogicalThreadContext.Properties["User"] = userby;
                _log.Error("Error : ", e);
                return Ok(new { Status = st });
            }
        }

        [AllowAnonymous]
        [HttpGet("GetAppVersion")]
        public async Task<IActionResult> GetAppVersion()
        {
            try
            {
                var dt = new AppVersionInfo();
                using (_context = new DapperContext())
                {
                    _uow = new UnitOfWork(_context);
                    dt = await _uow.AuthRepository.GetAppVersions();

                    if (dt == null)
                        return Unauthorized();
                }

                var st = StTrans.SetSt(200, 0, "Data di temukan");
                return Ok(new { Status = st, Results = dt });
            }
            catch (Exception e)
            {
                var st = StTrans.SetSt(400, 0, e.Message);
                _log.Error(e.Message);
                return Ok(new { Status = st });
            }
        }

        [HttpPost("ChangeToken"),Authorize]
        public async Task<IActionResult> ChangeToken(User user)
        {
            try
            {
                string version = _httpContext.HttpContext.User.FindFirst(ClaimTypes.Version).Value;

                using (_context = new DapperContext())
                {
                    _uow = new UnitOfWork(_context);
                    var flag = await _uow.AuthRepository.UserExists(user.User_id);
                }

                var dt = GenerateJwtToken(user, version);
                user.Token = dt;

                var st = StTrans.SetSt(200, 0, "User Berhasil Login");
                return Ok(new { Status = st, Results = user });
            }
            catch (Exception ex)
            {
                var st = StTrans.SetSt(400, 0, ex.Message);
                _log.Error(ex.Message);
                return Ok(new { Status = st });
            }

        }
        private string GenerateJwtToken(User user, string lastVersion)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.PrimarySid, user.Sign_id),
                new Claim(ClaimTypes.Name, user.User_id),
                new Claim(ClaimTypes.Role, user.Role_id),
                new Claim(ClaimTypes.Version, lastVersion),
                new Claim(ClaimTypes.GroupSid, user.Company_id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _config.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
}
}