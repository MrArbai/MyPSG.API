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
using MyPSG.API.Dto.Auth;
using MyPSG.API.Models;
using MyPSG.API.Models.Auth;
using MyPSG.API.Repository.Implements;
using MyPSG.API.Repository.Interfaces;

namespace MyPSG.API.Controllers.Auth
{
    [Route("opiumapi/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(RoleController));
        private IUnitOfWork _uow;
        private IDapperContext _context;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContext;
        public RoleController(IConfiguration config)
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
                IEnumerable<Role> hasil;
                using(_context = new DapperContext()){
                    _uow = new UnitOfWork(_context);
                    hasil = await _uow.RoleRepository.GetAll();
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

        [HttpGet("GetRoleMenuByName"),Authorize]
        public async Task<IActionResult> GetRoleMenuByName(string role_id, string nama_menu, int grant_id)
        {
            try
            {
                Role hasil;
                using(_context = new DapperContext()){
                    _uow = new UnitOfWork(_context);
                    hasil = await _uow.RoleRepository.GetRoleByMenuNameAndGrant(role_id, nama_menu, grant_id);
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

        [HttpGet("GetRoleMenuByID"),Authorize]
        public async Task<IActionResult> GetRoleMenuByID(string role_id, string menu_id, int grant_id)
        {
            try
            {
                Role hasil;
                using(_context = new DapperContext()){
                    _uow = new UnitOfWork(_context);
                    hasil = await _uow.RoleRepository.GetRoleByMenuIDAndGrant(role_id, menu_id, grant_id);
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

        [HttpPost("GetByParam"),Authorize]
        public async Task<IActionResult> GetByParam(RoleDto param)
        {
            try
            {
                IEnumerable<Role> hasil;
                using(_context = new DapperContext()){
                    _uow = new UnitOfWork(_context);
                    hasil = await _uow.RoleRepository.GetByParam(param);
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

        [HttpPost("Save"),Authorize]   
        public async Task<IActionResult> Save(Role param){
            try
            {
                using(_context = new DapperContext()){
                    param.role_id ??= _context.GetGUID();
                    _uow = new UnitOfWork(_context);
                    await _uow.RoleRepository.Save(param);
                }

                var st2 = StTrans.SetSt(200, 0, "Role has been Created !");
                return Ok(new { Status = st2, Results = param });
            }
            catch (Exception e)
            {
                var st = StTrans.SetSt(400, 0, e.Message);
                return Ok(new { Status = st });
            }
        }

        [HttpPost("Update"),Authorize]   
        public async Task<IActionResult> Update(Role param){
            try
            {
                using(_context = new DapperContext()){
                    _uow = new UnitOfWork(_context);
                    await _uow.RoleRepository.Update(param);
                }

                var st2 = StTrans.SetSt(200, 0, "Role has been Updated !");
                return Ok(new { Status = st2, Results = param });
            }
            catch (Exception e)
            {
                var st = StTrans.SetSt(400, 0, e.Message);
                return Ok(new { Status = st });
            }
        }

        [HttpPost("Delete"),Authorize]   
        public async Task<IActionResult> Delete(Role param){
            try
            {
                using(_context = new DapperContext()){
                    _uow = new UnitOfWork(_context);
                    await _uow.RoleRepository.Delete(param);
                }

                var st2 = StTrans.SetSt(200, 0, "Role has been Deleted !");
                return Ok(new { Status = st2, Results = param });
            }
            catch (Exception e)
            {
                var st = StTrans.SetSt(400, 0, e.Message);
                return Ok(new { Status = st });
            }
        }
    }
}