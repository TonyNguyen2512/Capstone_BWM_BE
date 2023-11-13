﻿using Data.Models;
using Data.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.UserService;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("[action]"), Authorize]
        public async Task<ActionResult> CreateAdmin([FromBody] UserCreateModel model)
        {
            if (string.IsNullOrEmpty(model.email)) return BadRequest("Không nhận được Email!");
            if (string.IsNullOrEmpty(model.phoneNumber)) return BadRequest("Không nhận được số điện thoại!");
            if (string.IsNullOrEmpty(model.fullName)) return BadRequest("Không nhận được họ tên!");
            if (model.password.Length < 6) return BadRequest("Mật khẩu phải nhiều hơn 6 ký tự !");        
            var result = await _userService.CreateAdmin(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage});         
        }
        
        [HttpPost("[action]")]
        public async Task<ActionResult> CreateForeman([FromBody] UserCreateModel model)
        {
            if (string.IsNullOrEmpty(model.email)) return BadRequest("Không nhận được Email!");
            if (string.IsNullOrEmpty(model.phoneNumber)) return BadRequest("Không nhận được số điện thoại!");
            if (string.IsNullOrEmpty(model.fullName)) return BadRequest("Không nhận được họ tên!");
            if (model.password.Length < 6) return BadRequest("Mật khẩu phải nhiều hơn 6 ký tự !");
            var result = await _userService.CreateForeman(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> CreateLeader([FromBody] UserCreateModel model)
        {
            if (string.IsNullOrEmpty(model.email)) return BadRequest("Không nhận được Email!");
            if (string.IsNullOrEmpty(model.phoneNumber)) return BadRequest("Không nhận được số điện thoại!");
            if (string.IsNullOrEmpty(model.fullName)) return BadRequest("Không nhận được họ tên!");
            if (model.password.Length < 6) return BadRequest("Mật khẩu phải nhiều hơn 6 ký tự !");
            var result = await _userService.CreateLeader(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> CreateWorker([FromBody] UserCreateModel model)
        {
            if (string.IsNullOrEmpty(model.email)) return BadRequest("Không nhận được Email!");
            if (string.IsNullOrEmpty(model.phoneNumber)) return BadRequest("Không nhận được số điện thoại!");
            if (string.IsNullOrEmpty(model.fullName)) return BadRequest("Không nhận được họ tên!");
            if (model.password.Length < 6) return BadRequest("Mật khẩu phải nhiều hơn 6 ký tự !");
            var result = await _userService.CreateWorker(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Login([FromBody] LoginModel model)
        {
            if (string.IsNullOrEmpty(model.phoneNumber)) return BadRequest("Không nhận được số điện thoại!");
            if (string.IsNullOrEmpty(model.password)) return BadRequest("Không nhận được mật khẩu!");
            var result = await _userService.Login(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]")]
        public IActionResult GetAll(string? search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result =  _userService.GetAll(search,pageIndex,pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("{phoneNumber}")]
        public IActionResult GetByEmail(string phoneNumber)
        {
            var result = _userService.GetByPhoneNumber(phoneNumber);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetById(Guid id)
        {
            var result = _userService.GetById(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]")]
        public IActionResult GetRole()
        {
            var result = _userService.GetRole();
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut]
        public IActionResult Update(UserUpdateModel model)
        {
            var result = _userService.Update(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }        

        [HttpPut("[action]")]
        public IActionResult UpdatePhone(UserUpdatePhoneModel model)
        {
            var result = _userService.UpdatePhone(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]")]
        public IActionResult UpdateRole(UserUpdateUserRoleModel model)
        {
            var result = _userService.UpdateRole(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> ChangePassword([FromBody] UserUpdatePasswordModel model)
        {
            var result = await _userService.ChangePassword(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]/{id}")]
        public IActionResult BanUser(Guid id)
        {
            var result = _userService.BannedUser(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]/{id}")]
        public IActionResult UnBanUser(Guid id)
        {
            var result = _userService.UnBannedUser(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }
    }
}
