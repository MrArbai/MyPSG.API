using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyPSG.API.Dto.Master;
using MyPSG.API.Models;
using MyPSG.API.Models.Master;
using MyPSG.API.Repository.Implements;
using MyPSG.API.Repository.Implements.Master;
using MyPSG.API.Repository.Interfaces;

namespace MyPSG.API.Controllers.Master
{
    [Route("mypsgapi/[controller]")]
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
                IEnumerable<Item> hasil;
                using(IDapperContext _context = new DapperContext()){
                    var _uow = new UnitOfWorkMaster(_context);
                    hasil = await _uow.ItemRepository.GetAll();
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
        [HttpGet("GetByID"),Authorize]   
        public async Task<IActionResult> GetByID(string id){
            try
            {
                Item hasil = new();
                using(IDapperContext _context = new DapperContext()){
                    var _uow = new UnitOfWorkMaster(_context);
                    hasil = await _uow.ItemRepository.GetByID(id);
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
        [HttpGet("Get"),Authorize]   
        public async Task<IActionResult> Get(ItemParam param){
            try
            {
                IEnumerable<ItemDto> hasil;
                using(IDapperContext _context = new DapperContext()){
                    var _uow = new UnitOfWorkMaster(_context);
                    hasil = await _uow.ItemRepository.GetItem(param);
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