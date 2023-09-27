﻿using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Sevices.Core.MaterialService;

namespace WorkshopManagementSystem_BWM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : Controller
    {
        private readonly IMaterialService _materialService;

        public MaterialController(IMaterialService materialService)
        {
            _materialService = materialService;
        }

        [HttpPost("CreateMaterialCategory")]
        public async Task<ActionResult> CreateMaterialCategory(CreateMaterialCategoryModel model)
        {
            var result = await _materialService.CreateCategory(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("CreateMaterial")]
        public async Task<ActionResult> CreateMaterial(CreateMaterialModel model)
        {
            var result = await _materialService.CreateMaterial(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut]
        public IActionResult UpdateMaterialCategory(UpdateMaterialCategoryModel model)
        {
            var result = _materialService.UpdateMaterialCategory(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut]
        public IActionResult UpdateMaterial(UpdateMaterialModel model)
        {
            var result = _materialService.UpdateMaterial(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllMaterial()
        {
            var result = _materialService.GetAllMaterial();
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllMaterialCategory()
        {
            var result = _materialService.GetAllMaterial();
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("[action]/{id}")]
        public IActionResult DeleteMaterialCategory(int id)
        {
            var result = _materialService.DeleteCategory(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("[action]/{id}")]
        public IActionResult DeleteMaterial(int id)
        {
            var result = _materialService.DeleteMaterial(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
    }
}
