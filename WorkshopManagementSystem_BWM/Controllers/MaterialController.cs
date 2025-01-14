﻿using Data.Models;
using Data.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.MaterialService;
using System.Drawing.Printing;
using WorkshopManagementSystem_BWM.Extensions;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class MaterialController : Controller
    {
        private readonly IMaterialService _materialService;

        public MaterialController(IMaterialService materialService)
        {
            _materialService = materialService;
        }
        
        [HttpPost("[action]")]
        public ActionResult Create(CreateMaterialModel model)
        {
            if (!ValidateCreateMaterial(model))
            {
                return BadRequest(ModelState);
            }
            var result =  _materialService.Create(model, User.GetId());
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]")]
        public IActionResult GetAll(string? search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _materialService.GetAll(search, pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("[action]/{materialCategoryId}")]
        public IActionResult GetByMaterialCategoryId(Guid materialCategoryId, string? search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _materialService.GetByMaterialCategoryId(materialCategoryId,search, pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]/{orderId}")]
        public IActionResult GetByOrderId(Guid orderId, string? search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _materialService.GetByOrderId(orderId, search, pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetMaterialById(Guid id)
        {
            var result = _materialService.GetById(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpPut("[action]")]
        public IActionResult UpdateMaterial(UpdateMaterialModel model)
        {
            if (!ValidateUpdateMaterial(model))
            {
                return BadRequest(ModelState);
            }
            var result = _materialService.Update(model, User.GetId());
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpDelete("[action]/{id}")]
        public IActionResult Delete(Guid id)
        {
            var result = _materialService.Delete(id, User.GetId());
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(new ResponeResultModel { Code = result.Code, ErrorMessage = result.ErrorMessage });
        }

        [HttpGet("GetAllLogOnMaterial")]
        public IActionResult GetAllLogOnMaterial(string? search, int pageIndex = ConstPaging.Index, int pageSize = ConstPaging.Size)
        {
            var result = _materialService.GetAllLogOnMaterial(search, pageIndex, pageSize);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
        #region Validate
        private bool ValidateCreateMaterial(CreateMaterialModel model)
        {
            if (model.materialCategoryId == Guid.Empty)
            {
                ModelState.AddModelError(nameof(model.materialCategoryId),
                    $"Không nhận được {model.materialCategoryId}!");
            }
            if (string.IsNullOrWhiteSpace(model.name))
            {
                ModelState.AddModelError(nameof(model.name),
                    $"{model.name} không được để trống !");
            }
            if (string.IsNullOrWhiteSpace(model.color))
            {
                ModelState.AddModelError(nameof(model.color),
                    $"{model.color} không được để trống !");
            }
            if (string.IsNullOrWhiteSpace(model.supplier))
            {
                ModelState.AddModelError(nameof(model.supplier),
                    $"{model.supplier} không được để trống !");
            }
            if (model.thickness <= 0)
            {
                ModelState.AddModelError(nameof(model.thickness),
                    $"{model.thickness} nhỏ hơn hoặc bằng 0 !");
            }
            if (string.IsNullOrWhiteSpace(model.unit))
            {
                ModelState.AddModelError(nameof(model.unit),
                    $"{model.unit} không được để trống !");
            }         
            if (string.IsNullOrWhiteSpace(model.importPlace))
            {
                ModelState.AddModelError(nameof(model.importPlace),
                    $"{model.importPlace} không được để trống !");
            }            
            if (model.price <= 0)
            {
                ModelState.AddModelError(nameof(model.price),
                    $"{model.price} nhỏ hơn hoặc bằng 0 !");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }

        private bool ValidateUpdateMaterial(UpdateMaterialModel model)
        {
            if (model.materialCategoryId == Guid.Empty)
            {
                ModelState.AddModelError(nameof(model.materialCategoryId),
                    $"Không nhận được {model.materialCategoryId}!");
            }
            if (string.IsNullOrWhiteSpace(model.name))
            {
                ModelState.AddModelError(nameof(model.name),
                    $"{model.name} không được để trống !");
            }
            if (string.IsNullOrWhiteSpace(model.color))
            {
                ModelState.AddModelError(nameof(model.color),
                    $"{model.color} không được để trống !");
            }
            if (string.IsNullOrWhiteSpace(model.supplier))
            {
                ModelState.AddModelError(nameof(model.supplier),
                    $"{model.supplier} không được để trống !");
            }
            if (model.thickness <= 0)
            {
                ModelState.AddModelError(nameof(model.thickness),
                    $"{model.thickness} nhỏ hơn hoặc bằng 0 !");
            }
            if (string.IsNullOrWhiteSpace(model.unit))
            {
                ModelState.AddModelError(nameof(model.unit),
                    $"{model.unit} không được để trống !");
            }          
            if (string.IsNullOrWhiteSpace(model.importPlace))
            {
                ModelState.AddModelError(nameof(model.importPlace),
                    $"{model.importPlace} không được để trống !");
            }
            if (model.price <= 0)
            {
                ModelState.AddModelError(nameof(model.price),
                    $"{model.price} nhỏ hơn hoặc bằng 0 !");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
