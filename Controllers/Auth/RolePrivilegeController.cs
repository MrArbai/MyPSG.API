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
using Opium.Api.Dtos.Utl;
using Opium.Api.Models;
using Opium.Api.Models.Utl;
using Opium.Api.Repository.Implements;
using Opium.Api.Repository.Interfaces;

namespace Opium.Api.Controllers.Utl
{
    [Route("opiumapi/[controller]")]
    [ApiController]
    public class RolePrivilegeController : ControllerBase
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(RolePrivilegeController));
        private IUnitOfWork _uow;
        private IDapperContext _context;
        private readonly IConfiguration _config;
        private IHttpContextAccessor _httpContext;
        public RolePrivilegeController(IConfiguration config)
        {
            _config = config;
            _httpContext = (IHttpContextAccessor)new HttpContextAccessor();
        }

        [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                IEnumerable<RolePrivilege> hasil;
                using(IDapperContext _context = new DapperContext()){
                    var _uow = new UnitOfWork(_context);
                    hasil = await _uow.RolePrivilegeRepository.GetAll();
                }

                var st2 = StTrans.SetSt(200, 0, "Data di temukan");
                return Ok(new { Status = st2, Results = hasil });
            }
            catch (System.Exception e)
            {
                var st = StTrans.SetSt(400, 0, e.Message);
                return Ok(new { Status = st });
            }
        }


        [Authorize(Policy = "RequireAdmin")]
        [HttpGet("GetRoleMenuByName")]
        public async Task<IActionResult> GetRoleMenuByName(string role_id, string nama_menu, int grant_id)
        {
            try
            {
                RolePrivilege hasil;
                using(IDapperContext _context = new DapperContext()){
                    var _uow = new UnitOfWork(_context);
                    hasil = await _uow.RolePrivilegeRepository.GetRoleByMenuNameAndGrant(role_id, nama_menu, grant_id);
                }

                var st2 = StTrans.SetSt(200, 0, "Data di temukan");
                return Ok(new { Status = st2, Results = hasil });
            }
            catch (System.Exception e)
            {
                var st = StTrans.SetSt(400, 0, e.Message);
                return Ok(new { Status = st });
            }
        }

        [Authorize(Policy = "RequireAdmin")]
        [HttpGet("GetRoleMenuByID")]
        public async Task<IActionResult> GetRoleMenuByID(string role_id, string menu_id, int grant_id)
        {
            try
            {
                RolePrivilege hasil;
                using(IDapperContext _context = new DapperContext()){
                    var _uow = new UnitOfWork(_context);
                    hasil = await _uow.RolePrivilegeRepository.GetRoleByMenuIDAndGrant(role_id, menu_id, grant_id);
                }

                var st2 = StTrans.SetSt(200, 0, "Data di temukan");
                return Ok(new { Status = st2, Results = hasil });
            }
            catch (System.Exception e)
            {
                var st = StTrans.SetSt(400, 0, e.Message);
                return Ok(new { Status = st });
            }
        }

        [Authorize(Policy = "RequireAdmin")]
        [HttpGet("GetRoleByID")]
        public async Task<IActionResult> GetRoleByID(string role_id)
        {
            try
            {
                IEnumerable<RolePrivilege> hasil;
                using(IDapperContext _context = new DapperContext()){
                    var _uow = new UnitOfWork(_context);
                    hasil = await _uow.RolePrivilegeRepository.GetRoleByID(role_id);
                }

                var st2 = StTrans.SetSt(200, 0, "Data di temukan");
                return Ok(new { Status = st2, Results = hasil });
            }
            catch (System.Exception e)
            {
                var st = StTrans.SetSt(400, 0, e.Message);
                return Ok(new { Status = st });
            }
        }

        [Authorize(Policy="RequireAdmin")]        
        [HttpPost("Save")]
        public async Task<IActionResult> Save(RolePrivilege dt){
            string userby = _httpContext.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            try
            {
                //dt.CreatedDate = DateTime.Now;
                
                 using(_context = new DapperContext()){
                    _uow = new UnitOfWork(_context);
                    await _uow.RolePrivilegeRepository.SaveRole(dt); 
                }
               
                log4net.LogicalThreadContext.Properties["User"] = userby;
                _log.Info("Succes Save");
                
                var st = StTrans.SetSt(200, 0, "Succes");
                return Ok(new{Status = st, Results = dt});
        
            }
            catch (System.Exception e)
            {
                var st = StTrans.SetSt(400, 0, e.Message);

                log4net.LogicalThreadContext.Properties["User"] = userby;
                _log.Error("Error : ", e);
                return Ok(new{Status = st});
            }
        }        

        private string GenerateJwtToken(User user, string lastVersion)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.user_id),
                new Claim(ClaimTypes.Role, user.role_id),
                new Claim(ClaimTypes.Version, lastVersion)
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