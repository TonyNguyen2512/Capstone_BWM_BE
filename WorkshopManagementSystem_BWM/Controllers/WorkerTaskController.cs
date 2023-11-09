﻿using WorkshopManagementSystem_BWM.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.WorkerTaskService;
using static Data.Models.WorkerTaskModel;
using Data.Models;
using Data.Enums;
using Data.Utils;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerTaskController : ControllerBase
    {
        private readonly IWorkerTaskService _workerTaskService;

        public WorkerTaskController(IWorkerTaskService workerTaskService)
        {
            _workerTaskService = workerTaskService;
        }

        [HttpPost("[action]")]
        public IActionResult Create(CreateWorkerTaskModel model)
        {
            if (model.leaderTaskId == Guid.Empty) return BadRequest("Không nhận được công việc trưởng nhóm!");
            if (string.IsNullOrEmpty(model.description)) return BadRequest("Không nhận được mô tả!");
            var userId = User.GetId();
            var result = _workerTaskService.Create(userId, model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]/{leaderTaskId}")]
        public IActionResult GetAll(Guid leaderTaskId, string? search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _workerTaskService.GetByLeaderTaskId(leaderTaskId, search, pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetById(Guid id)
        {
            var result = _workerTaskService.GetById(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]")]
        public IActionResult Update(UpdateWorkerTaskModel model)
        {
            if (string.IsNullOrEmpty(model.name)) return BadRequest("Không nhận được tên công việc!");
            if (string.IsNullOrEmpty(model.description)) return BadRequest("Không nhận được mô tả!");
            var result = _workerTaskService.Update(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]/{id}/{status}")]
        public IActionResult UpdateStatus(Guid id, ETaskStatus status)
        {
            var result = _workerTaskService.UpdateStatus(id, status);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]")]
        public IActionResult Assign(AssignWorkerTaskModel model)
        {
            var result = _workerTaskService.Assign(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]")]
        public IActionResult UnAssign(AssignWorkerTaskModel model)
        {
            var result = _workerTaskService.UnAssign(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpDelete("[action]/{id}")]
        public IActionResult Delete(Guid id)
        {
            var result = _workerTaskService.Delete(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }       
    }
}