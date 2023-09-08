using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyPSG.API.Models;
using MyPSG.API.Models.Auth;
using MyPSG.API.Repository.Implements;
using MyPSG.API.Repository.Interfaces;

namespace MyPSG.API.Controllers.Auth
{
    [Route("mypsgapi/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(MenuController));
        private readonly IConfiguration _config;
        public MenuController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("GetAllMenu")]   
        public async Task<IActionResult> GetAllMenu(){
            try
            {
                IEnumerable<Menu> hasil;
                using(IDapperContext _context = new DapperContext()){
                    var _uow = new UnitOfWork(_context);
                    hasil = await _uow.MenuRepository.GetAllMenu();
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
        [HttpGet("GetAllHeader")]   
        public async Task<IActionResult> GetAllHeader(){
            try
            {
                IEnumerable<MenuHeader> hasil;
                using(IDapperContext _context = new DapperContext()){
                    var _uow = new UnitOfWork(_context);
                    hasil = await _uow.MenuRepository.GetAllHeader();
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
        [HttpGet("GetByMenuId")]   
        public async Task<IActionResult> GetByMenuId(int MenuId){
            try
            {
                var dt = new List<MenuItem>();
                using(IDapperContext _context = new DapperContext()){
                    var _uow = new UnitOfWork(_context);
                    var dt2 = await _uow.MenuItemRepository.GetByMenuId(MenuId);
                    dt = dt2.ToList();
                }

                var st2 = StTrans.SetSt(200, 0, "Data di temukan");
                return Ok(new { Status = st2, Results = dt });
            }
            catch (Exception e)
            {
                var st = StTrans.SetSt(400, 0, e.Message);
                return Ok(new { Status = st });
            }
        }        
    }
}