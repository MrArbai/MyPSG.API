using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyPSG.API.Models;
using MyPSG.API.Models.Auth;
using MyPSG.API.Repository.Implements;
using MyPSG.API.Repository.Interfaces;

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
            _httpContext = new HttpContextAccessor();
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
            catch (Exception e)
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
            catch (Exception e)
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
            catch (Exception e)
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
            catch (Exception e)
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
                
                 using(_context = new DapperContext()){
                    _uow = new UnitOfWork(_context);
                    await _uow.RolePrivilegeRepository.SaveRole(dt); 
                }
               
                LogicalThreadContext.Properties["User"] = userby;
                _log.Info("Succes Save");
                
                var st = StTrans.SetSt(200, 0, "Succes");
                return Ok(new{Status = st, Results = dt});
        
            }
            catch (Exception e)
            {
                var st = StTrans.SetSt(400, 0, e.Message);

                LogicalThreadContext.Properties["User"] = userby;
                _log.Error("Error : ", e);
                return Ok(new{Status = st});
            }
        }        
    }
}