﻿using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.CategoryService
{
    public interface IMaterialCategoryService
    {       
        ResultModel CreateMaterialCategory(Guid createById, CreateMaterialCategoryModel model);
        ResultModel GetAllMaterialCategory(string? search, int pageIndex, int pageSize);
        ResultModel GetMaterialCategoryById(Guid id);
        ResultModel UpdateMaterialCategory(UpdateMaterialCategoryModel model);
        ResultModel DeleteMaterialCategory(Guid id);
    }
}
