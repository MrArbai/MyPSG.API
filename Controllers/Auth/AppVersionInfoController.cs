using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyPSG.API.Models;
using MyPSG.API.Models.Auth;
using MyPSG.API.Repository.Implements;
using MyPSG.API.Repository.Interfaces;

namespace Opium.Api.Controllers.Utl
{
    [Route("opiumapi/[controller]")]
    [ApiController]
    public class AppVersionInfoController : ControllerBase
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(AppVersionInfoController));
        private IUnitOfWork _uow;
        private IDapperContext _context;
        private readonly IConfiguration _config;
        private IHttpContextAccessor _httpContext;

        public AppVersionInfoController(IConfiguration config)
        {
            _config = config;
            _httpContext = new HttpContextAccessor();
        }

        [AllowAnonymous]
        [HttpGet("GetLastAppVersion")]
        public async Task<IActionResult> GetLastAppVersion()
        {
            try
            {

                var dt = new AppVersionInfo();
                using (_context = new DapperContext())
                {
                    _uow = new UnitOfWork(_context);
                    dt = await _uow.AppVersionInfoRepository.GetLastAppVersion();
                }

                var st = StTrans.SetSt(200, 0, "Succes");
                return Ok(new { Status = st, Results = dt });
            }
            catch (Exception e)
            {
                var st = StTrans.SetSt(400, 0, e.Message);
                _log.Error("Error : ", e);
                return Ok(new { Status = st });
            }
        }

        [Authorize(Policy = "RequireAdmin")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var dt = new List<AppVersionInfo>();
                using (_context = new DapperContext())
                {
                    _uow = new UnitOfWork(_context);
                    var dt2 = await _uow.AppVersionInfoRepository.GetAll();
                    dt = dt2.ToList();
                }

                var st = StTrans.SetSt(200, 0, "Succes");
                return Ok(new { Status = st, Results = dt });
            }
            catch (Exception e)
            {
                var st = StTrans.SetSt(400, 0, e.Message);
                _log.Error("Error : ", e);
                return Ok(new { Status = st });
            }
        }

        [HttpPost("Save"),Authorize]
        public async Task<IActionResult> Save(AppVersionInfo dt)
        {
            string userby = _httpContext.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            try
            {
                using (_context = new DapperContext())
                {
                    _uow = new UnitOfWork(_context);
                    dt.created_by = userby;
                    dt.created_date = DateTime.Now;
                    await _uow.AppVersionInfoRepository.Save(dt);
                }

                LogicalThreadContext.Properties["NewValue"] = Logs.ToJson(dt);
                LogicalThreadContext.Properties["User"] = userby;
                _log.Info("Succes Save");

                var st = StTrans.SetSt(200, 0, "Succes");
                return Ok(new { Status = st, Results = dt });

            }
            catch (Exception e)
            {
                var st = StTrans.SetSt(400, 0, e.Message);
                LogicalThreadContext.Properties["NewValue"] = Logs.ToJson(dt);
                LogicalThreadContext.Properties["User"] = userby;
                _log.Error("Error : ", e);
                return Ok(new { Status = st });
            }
        }

        [HttpPost("Update"),Authorize]
        public async Task<IActionResult> Update(AppVersionInfo dt)
        {
            string userby = _httpContext.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            try
            {
                using (_context = new DapperContext())
                {
                    _uow = new UnitOfWork(_context);
                    await _uow.AppVersionInfoRepository.Update(dt);
                }
                LogicalThreadContext.Properties["NewValue"] = Logs.ToJson(dt);
                LogicalThreadContext.Properties["User"] = userby;
                _log.Info("Succes Update");

                var st = StTrans.SetSt(200, 0, "Succes");
                return Ok(new { Status = st, Results = dt });
            }
            catch (Exception e)
            {
                var st = StTrans.SetSt(400, 0, e.Message);
                LogicalThreadContext.Properties["NewValue"] = Logs.ToJson(dt);
                LogicalThreadContext.Properties["User"] = userby;
                _log.Error("Error : ", e);
                return Ok(new { Status = st });
            }
        }

        [HttpPost("Upload"),Authorize]
        [RequestFormLimits(MultipartBodyLengthLimit = 409715200)]
        [RequestSizeLimit(409715200)]
        public async Task<IActionResult> Upload([FromForm] AppVersionInfoXml obj)
        {
            var dt = new AppVersionInfo();
            var objXml = new AppVersionInfoXml();

            string userby = _httpContext.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            try
            {
                string namaFile = "";

                if (obj.File != null && obj.File.Length > 0)
                {
                    namaFile = obj.last_version == null ? "" : Path.GetFileNameWithoutExtension(obj.File.FileName)  + "_" + obj.last_version + Path.GetExtension(obj.File.FileName);
                }

                if (namaFile != "")
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Updater", namaFile);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await obj.File.CopyToAsync(stream);

                        objXml.app_id = obj.app_id;
                        objXml.last_version = obj.last_version;
                        objXml.FileName = namaFile;
                        objXml.Url = obj.Url;
                        objXml.MD5 = obj.MD5;
                        objXml.bug_fixed = obj.bug_fixed;
                        objXml.LaunchArgs = obj.LaunchArgs;

                        CreateXmlFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Updater"), "update.xml", objXml);

                        objXml.Url = obj.UrlInet;

                        CreateXmlFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Updater"), "updateInet.xml", objXml);
                    }
                }

                using (_context = new DapperContext())
                {
                    _uow = new UnitOfWork(_context);
                    await _uow.AppVersionInfoRepository.UpdateVersion(obj.id, namaFile);
                }

                LogicalThreadContext.Properties["NewValue"] = Logs.ToJson(obj);
                LogicalThreadContext.Properties["User"] = userby;
                _log.Info("Succes Upload");

                var st = StTrans.SetSt(200, 0, "Succes");
                return Ok(new { Status = st, Results = dt });
            }
            catch (System.Exception e)
            {
                var st = StTrans.SetSt(400, 0, e.Message);
                LogicalThreadContext.Properties["NewValue"] = Logs.ToJson(obj);
                LogicalThreadContext.Properties["User"] = userby;
                _log.Error("Error : ", e);
                return Ok(new { Status = st });
            }
        }

        private static void CreateXmlFile(string dir, string fileName, AppVersionInfoXml obj)
        {
            XmlDocument doc = new XmlDocument();
            
            using(XmlWriter writer = doc.CreateNavigator().AppendChild())
            {
                // Do this directly
                writer.WriteStartDocument();
                writer.WriteStartElement("sharpUpdate");
                writer.WriteStartElement("update");
                writer.WriteAttributeString("appId", obj.app_id.ToString());
                writer.WriteElementString("version", obj.last_version);
                writer.WriteElementString("url", obj.Url);
                writer.WriteElementString("fileName", obj.FileName);
                writer.WriteElementString("md5", obj.MD5);
                writer.WriteElementString("description", obj.bug_fixed);
                writer.WriteElementString("launchArgs", obj.LaunchArgs);
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();
            }
            
            doc.Save(string.Format("{0}//{1}",dir,fileName));

           
        }
 
        [AllowAnonymous]
        [HttpGet("GetDate")]
        public IActionResult GetDate(){
            var st = StTrans.SetSt(200, 0, "Succes");
            var dt = new {Date = DateTime.Now};
            return Ok(new { Status = st, Results = dt });
        }

        private string GetContentType(string path)
        {
            
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>{
                {".txt","text/plain"},
                {".pdf","application/pdf"},
                {".doc","application/vnd.ms-word"},
                {".docx","application/vnd.ms-word"},
                {".xls","application/vnd.ms-excel"},
                {".xlsx","application/vnd.ms-openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png","image/png"},
                {".jpg","image/jpeg"},
                {".jpeg","image/jpeg"},
                {".gif","image/gif"},
                {".csv","text/csv"},
                {".xml", "application/xml"},
                {".exe", "application/octet-stream"},
                {".zip", "application/x-zip-compressed"},
                {".msi", "application/x-msi"}

            };
        }


        
        [AllowAnonymous]
        [HttpGet("GetAPIVersion")]
        public async Task<IActionResult> GetAPIVersion()
        {
            string v = "";
            var dt = new AppVersionInfo();
            using (_context = new DapperContext())
            {
                _uow = new UnitOfWork(_context);
                dt = await _uow.AppVersionInfoRepository.GetLastAppVersion();
            }

            try
            {
                var st2 = StTrans.SetSt(200, 0, "Last Update 15/01/2022 00:11");
                return Ok(new { Status = st2, Results = v });
                
            }
            catch (System.Exception)
            {
                throw;
            }
        }

    }
}