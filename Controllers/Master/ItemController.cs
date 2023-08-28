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
    [Route("opiumapi/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ItemController));
        private readonly IConfiguration _config;
        public ItemController(IConfiguration config)
        {
            _config = config;
        }
        
        [HttpGet("GetAllItem"),Authorize]   
        public async Task<IActionResult> GetAll(){
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
   
    }
}