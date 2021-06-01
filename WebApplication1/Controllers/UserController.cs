using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Threading.Tasks;
using Core.Common.ErrorLogging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using School.Business.Contract;
using School.ViewModel;

namespace School.WebLayer.Controllers
{
    [ApiController]
    [Route("api/h{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class UserController : BaseController
    {
        private readonly IUserBusinessMgr _userBusinessMgr;
        private ILog _logger;
        public UserController(IUserBusinessMgr userBusinessMgr, ILog logger)
        {
            _userBusinessMgr = userBusinessMgr;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateRequestViewModel model)
        {
            var response = _userBusinessMgr.Authenticate(model, GetIpAddress());

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            SetTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        [Route("refresh-token")]
        public IActionResult RefreshToken([FromBody] RequestRefreshTokenViewModel refreshTokenTestViewModel)
        {
            //var bodyString = HttpContext.Items["request_body"];
            //var value = JValue.Parse(bodyString.ToString());
            var response = _userBusinessMgr.RefreshToken(refreshTokenTestViewModel.refreshToken, GetIpAddress());

            if (response == null)
                return Unauthorized(new { message = "Invalid token" });

            SetTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        [Route("revoke-token")]
        public IActionResult RevokeToken([FromBody] RevokeTokenRequestViewModel model)
        {
            // accept token from request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            var response = _userBusinessMgr.RevokeToken(token, GetIpAddress());

            if (!response)
                return NotFound(new { message = "Token not found" });

            return Ok(new { message = "Token revoked" });
        }

        [Authorize]
        [HttpGet]
        [Route("GetAllUsers")]
        public IActionResult GetAllUsers(int? roleId)
        {
            List<UserDetailViewModel> userList = new List<UserDetailViewModel>();
            try
            {
                userList = _userBusinessMgr.GetAllUsers(roleId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return Ok(userList);
        }

        [Authorize]
        [HttpGet]
        [Route("GetUserById")]
        public IActionResult GetUserById([FromQuery] int id)
        {
            UserViewModel user = _userBusinessMgr.GetUserById(id);
            if (user == null) return NotFound();

            return Ok(user);
        }

        [Authorize]
        [HttpGet]
        [Route("GetRefreshTokens")]
        public IActionResult GetRefreshTokens([FromQuery] int id)
        {
            var user = _userBusinessMgr.GetUserById(id);
            if (user == null) return NotFound();

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("RegisterUserDetail")]
        public IActionResult RegisterUserDetail([FromBody] UserDetailViewModel userDetailViewModel)
        {
            int regUserDetail = _userBusinessMgr.RegisterUserDetail(userDetailViewModel);
            if (regUserDetail == 0) return BadRequest();
            return Ok("User Registration has been successful.");
        }

        [Authorize]
        [HttpGet("GetAllRoles")]
        public IActionResult GetAllRoles()
        {
            return Ok(_userBusinessMgr.GetAllRoles());
        }

        [Authorize]
        [HttpGet("GetAllCountries")]
        public IActionResult GetAllCountries()
        {
            return Ok(_userBusinessMgr.GetAllCountries());
        }

        [Authorize]
        [HttpGet("GetAllState")]
        public IActionResult GetAllState()
        {
            return Ok(_userBusinessMgr.GetAllStates());
        }

        [Authorize]
        [HttpGet("GetAllCities")]
        public IActionResult GetAllCities()
        {
            return Ok(_userBusinessMgr.GetAllCities());
        }

        [Authorize]
        [HttpGet("GetAllBoards")]
        public IActionResult GetAllBoards()
        {
            return Ok(_userBusinessMgr.GetAllBoards());
        }

        [Authorize]
        [HttpGet("GetAllGrades")]
        public IActionResult GetAllGrades()
        {
            return Ok(_userBusinessMgr.GetAllGrades());
        }

        [Authorize]
        [HttpGet("GetAllSubjects")]
        public IActionResult GetAllSubjects()
        {
            return Ok(_userBusinessMgr.GetAllSubjects());
        }

        [Authorize]
        [HttpPost]
        [Route("CreateUserRoleSchedule")]
        public IActionResult CreateUserRoleSchedule([FromForm] UserRoleScheduleFileUpload userRoleScheduleFileUpload)
        {
            try
            {
                List<UserRoleScheduleViewModel> userRoleSchedule = JsonConvert.DeserializeObject<List<UserRoleScheduleViewModel>>(userRoleScheduleFileUpload.userRoleSchedule);
                string folderName = Path.Combine("Resources", "UserRoleScheduleDocs");
                string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                userRoleSchedule.ForEach(a => a.TopicRefMaterialBasePath = pathToSave);
                List<int> scheduleIdList = _userBusinessMgr.CreateUserRoleSchedule(userRoleSchedule);
                bool fileSaveStatus = UploadUserRoleScheduleFiles(scheduleIdList);
                if (!fileSaveStatus)
                    return BadRequest();
                else
                    return Ok("Schedule has been saved successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        //[Authorize]
        //[HttpPost]
        //[Route("CreateUserRoleSchedule")]
        //public IActionResult CreateUserRoleSchedule([FromBody] UserRoleScheduleFileUpload userRoleScheduleFileUpload)
        //{
        //    try
        //    {
        //        //List<UserRoleScheduleViewModel> userRoleSchedule = JsonConvert.DeserializeObject<List<UserRoleScheduleViewModel>>(userRoleScheduleFileUpload.userRoleSchedule);
        //        List<UserRoleScheduleViewModel> userRoleSchedule = userRoleScheduleFileUpload.userRoleSchedule;
        //        //string folderName = Path.Combine("Resources", "UserRoleScheduleDocs");
        //        //string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        //        //userRoleSchedule.ForEach(a => a.TopicRefMaterialBasePath = pathToSave);
        //        List<int> scheduleIdList = _userBusinessMgr.CreateUserRoleSchedule(userRoleSchedule);
        //        //bool fileSaveStatus = UploadUserRoleScheduleFiles(scheduleIdList);
        //        bool fileSaveStatus = true;
        //        //throw new AuthenticationException("Message here");
        //        if (!fileSaveStatus)
        //            return BadRequest();
        //        else
        //            return Ok("Schedule has been saved successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        UpdateDbErrorLog(ex.Message.Substring(0, 8500));
        //        return StatusCode(500, $"Internal server error: {ex}");
        //    }
        //}

        public void UpdateDbErrorLog(string message)
        {
            _userBusinessMgr.UpdateDbErrorLog(message);
        }

        private bool UploadUserRoleScheduleFiles(List<int> scheduleIdList)
        {
            bool fileSaveStatus = false;
            foreach (var item in scheduleIdList)
            {
                foreach (var file in Request.Form.Files)
                {
                    var folderName = Path.Combine("Resources", "UserRoleScheduleDocs", item.ToString());
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    //var pathToSave = Path.Combine(_hostingEnvironment.WebRootPath, folderName);

                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }

                    if (file.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var fullPath = Path.Combine(pathToSave, fileName);
                        var dbPath = Path.Combine(folderName, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        fileSaveStatus = true;
                    }
                    else
                    {
                        fileSaveStatus = false;
                    }
                }
            }
            return fileSaveStatus;
        }

        [Authorize]
        [HttpPost]
        [Route("UpdateUserRoleSchedule")]
        public IActionResult UpdateUserRoleSchedule([FromForm] UserRoleScheduleFileUpload userRoleScheduleFileUpload)
        {
            try
            {
                List<UserRoleScheduleViewModel> userRoleSchedule = JsonConvert.DeserializeObject<List<UserRoleScheduleViewModel>>(userRoleScheduleFileUpload.userRoleSchedule);

                foreach (var item in userRoleSchedule)
                {
                    var folderName = Path.Combine("Resources", "UserRoleScheduleDocs", item.UserRoleScheduleId.ToString());
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    if (Directory.Exists(pathToSave))
                    {
                        Directory.Delete(pathToSave, true);
                    }
                }

                List<int> scheduleIdList = _userBusinessMgr.UpdateUserRoleSchedule(userRoleSchedule);
                bool fileSaveStatus = UploadUserRoleScheduleFiles(scheduleIdList);
                if (!fileSaveStatus)
                    return BadRequest();
                else
                    return Ok("Schedule has been updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        //[Authorize]
        //[HttpPost]
        //[Route("UpdateUserRoleSchedule")]
        //public IActionResult UpdateUserRoleSchedule([FromBody] UserRoleScheduleFileUpload userRoleScheduleFileUpload)
        //{
        //    try
        //    {
        //        List<UserRoleScheduleViewModel> userRoleSchedule = userRoleScheduleFileUpload.userRoleSchedule;
        //        List<int> scheduleIdList = _userBusinessMgr.UpdateUserRoleSchedule(userRoleSchedule);
        //        //bool fileSaveStatus = UploadUserRoleScheduleFiles(scheduleIdList);
        //        bool fileSaveStatus = true;
        //        if (!fileSaveStatus)
        //            return BadRequest();
        //        else
        //            return Ok("Schedule has been updated successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex}");
        //    }
        //}

        [Authorize]
        [HttpPost]
        [Route("GetUserRoleSchedule")]
        public IActionResult GetUserRoleSchedule(UserRoleScheduleRequestViewModel userRoleScheduleRequestViewModel)
        {
            List<UserRoleScheduleViewModel> userRoleScheduleList = _userBusinessMgr.GetUserRoleSchedule(userRoleScheduleRequestViewModel);
            return Ok(userRoleScheduleList);
        }

        [Authorize]
        [HttpPost]
        [Route("GetScheduleDoc")]
        public async Task<IActionResult> GetScheduleDoc(UserRoleScheduleDocReqViewModel userRoleScheduleDocReqViewModel)
        {
            if (userRoleScheduleDocReqViewModel.UserRoleScheduleDocName == null)
                return Content("Filename not present");

            string filename = userRoleScheduleDocReqViewModel.UserRoleScheduleDocName;

            string folderName = Path.Combine("Resources", "UserRoleScheduleDocs", userRoleScheduleDocReqViewModel.UserRoleScheduleId.ToString());
            string path = Path.Combine(Directory.GetCurrentDirectory(), folderName, filename);
            if (!System.IO.File.Exists(path))
                return BadRequest("File not present");

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        //[AllowAnonymous]
        //[HttpGet("CheckException")]
        //public IEnumerable<string> CheckException()
        //{
        //    throw new Exception();
        //}

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet"},  
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        private string GetIpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                //Expires = DateTime.UtcNow.AddDays(7)
                Expires = DateTime.UtcNow.AddSeconds(10)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}