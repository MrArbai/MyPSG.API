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
using Telegram.Bot;

namespace MyPSG.API.Controllers.Utl
{
    [Route("opiumapi/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(AuthController));
        private IUnitOfWork _uow;
        private IDapperContext _context;
        private readonly IConfiguration _config;
        private IHttpContextAccessor _httpContext;
        private TelegramBotClient Bot;

        public AuthController(IConfiguration config)
        {
            _config = config;
            _httpContext = new HttpContextAccessor();
        }

        [Authorize(Policy = "RequireAdmin")]
        [HttpPost("Save")]
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
                        Password_key = _context.GetGUID(),
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
            catch (System.Exception e)
            {
                var st = StTrans.SetSt(400, 0, e.Message);
                return Ok(new { Status = st });
            }
        }
        [Authorize(Policy = "RequireAdmin")]
        [HttpPost("Update")]
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

                    UserLoginInfo users = new()
                    {
                        Login_guid = _context.GetGUID(),
                        Login_date = DateTime.Now,
                        Login_id = userDto.User_id.ToLower(),
                        Computer_name = userDto.Computer,
                        Login_type = "O",
                        App_version = ds.last_version,
                        Ip_address = Request.HttpContext.Connection.RemoteIpAddress.ToString()
                    };

                    await _uow.AuthRepository.Login(users);
                    dt.Sign_id = users.Login_guid;

                    if (dt == null)
                        return Unauthorized();

                    dt.Role = await _uow.AuthRepository.GetRoleByID(dt.Role_id);
                    dt.Token = GenerateJwtToken(dt, ds.last_version);

                    IEnumerable<RolePrivilege> hasil;
                    hasil = await _uow.RolePrivilegeRepository.GetAll();

                    dt.RolePrivileges = hasil.ToList();
                }

                var st = StTrans.SetSt(200, 0, "User Berhasil Login");
                Random rnd = new Random();
                dt.Otp = rnd.Next(100000, 999999);

                Console.Write(dt.Telegram_id);
                if(dt.Telegram_id != null){
                    Bot = new TelegramBotClient(dt.Bot_token);
                    await Bot.SendTextMessageAsync(dt.Telegram_id, "Your OTP IS : " + dt.Otp);
                }
                
                return Ok(new { Status = st, Results = dt });
            }
            catch (System.Exception e)
            {
                var st = StTrans.SetSt(400, 0, e.Message);
                _log.Error(e.Message);
                return Ok(new { Status = st });
            }
        }

        [Authorize(Policy = "RequireAdmin")]
        [HttpPost("ChangePassword")]
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

        [Authorize(Policy = "RequireAdmin")]
        [HttpPost("ChangeToken")]
        public async Task<IActionResult> ChangeToken(User user)
        {
            try
            {
                string version =  _httpContext.HttpContext.User.FindFirst(ClaimTypes.Version).Value;

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
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.PrimarySid, user.Sign_id),
                new Claim(ClaimTypes.Name, user.User_id),
                new Claim(ClaimTypes.Role, user.Role_id),
                new Claim(ClaimTypes.Version, lastVersion),
                new Claim(ClaimTypes.GroupSid, user.Company_id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}