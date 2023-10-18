﻿using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevices.Core.ManagerTaskService
{
    public class ManagerTaskService : IManagerTaskService
    {
        private readonly AppDbContext _dbContext;

        public ManagerTaskService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultModel> CreatedManagerTask(Guid createById, CreateManagerTaskModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;            

            var check = await _dbContext.User.FindAsync(model.managerId);
            if (check == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "ManagerId không hợp lệ!";
                return result;
            }
            else
            {
                var orderTmp = await _dbContext.Order.FindAsync(model.orderId);
                if (orderTmp == null)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "OrderId không hợp lệ!";
                    return result;
                }
                else 
                {
                    var check1 = await _dbContext.ManagerTask.SingleOrDefaultAsync(a => a.name == model.name && a.orderId == model.orderId && a.isDeleted == false);

                    if (check1 != null)
                    {
                        result.Succeed = false;
                        result.ErrorMessage = "Công việc đã được tạo";
                        return result;
                    }
                    else 
                    {
                        if (orderTmp.status != Data.Enums.OrderStatus.InProgress)
                        {
                            result.Succeed = false;
                            result.ErrorMessage = "Đơn hàng đang không tiến hành";
                            return result;
                        }

                        else
                        {
                            if (model.startTime >= model.endTime)
                            {
                                result.Succeed = false;
                                result.ErrorMessage = "Ngày bắt đầu không thể lớn hơn hoặc bằng ngày kết thúc!";
                                return result;
                            }
                            else
                            {
                                var managerTask = new ManagerTask
                                {
                                    managerId = model.managerId,
                                    createById = createById,
                                    orderId = model.orderId,
                                    name = model.name,
                                    startTime = model.startTime,
                                    endTime = model.endTime,
                                    description = model.description,
                                    status = 0,
                                    isDeleted = false
                                };

                                try
                                {
                                    await _dbContext.ManagerTask.AddAsync(managerTask);
                                    await _dbContext.SaveChangesAsync();
                                    result.Succeed = true;
                                    result.Data = managerTask.id;
                                }
                                catch (Exception ex)
                                {
                                    result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                                }
                                return result;
                            }                           
                        }                       
                    }         
                }               
            }           
        }

        public async Task<ResultModel> UpdateManagerTask(UpdateManagerTaskModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var managerTask = await _dbContext.ManagerTask.FindAsync(model.id);
            if (managerTask == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy MannagerTask!";
                return result;
            }
            else
            {                                               
                if (model.startTime > model.endTime)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Ngày bắt đầu không thể lớn hơn ngày kết thúc!";
                    return result;
                }
                else
                {
                    managerTask.name = model.name;
                    managerTask.startTime = model.startTime;
                    managerTask.endTime = model.endTime;
                    managerTask.status = model.status;
                    managerTask.description = model.description;

                    try
                    {
                        await _dbContext.SaveChangesAsync();
                        result.Succeed = true;
                        result.Data = managerTask.id;
                    }
                    catch (Exception ex)
                    {
                        result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                    return result;
                }             
            }      
        }

        public async Task<List<ResponseManagerTaskModel>> GetManagerTaskByOrderId(Guid orderId)
        {
            var result = new List<ResponseManagerTaskModel>();
            var managerTask = await _dbContext.ManagerTask.Where(a => a.orderId == orderId && a.isDeleted == false).ToListAsync();
            if (managerTask == null) { 
                return null; 
            }
            else
            {
                foreach (var item in managerTask)
                {
                    var orderTmp = await _dbContext.Order.FindAsync(item.orderId);
                    var managerTmp = await _dbContext.User.FindAsync(item.managerId);
                    var createByTmp = await _dbContext.User.FindAsync(item.createById);
                    var tmp = new ResponseManagerTaskModel
                    {
                        createByName = createByTmp.fullName,
                        managerName = managerTmp.fullName,
                        orderName = orderTmp.name,
                        createdById = item.createById,
                        name = item.name,
                        startTime = item.startTime,
                        endTime = item.endTime,
                        status = item.status,
                        completedTime = item.completedTime,
                        description = item.description,
                        isDeleted = item.isDeleted,
                    };
                    result.Add(tmp);
                }
                return result;
            }
            
        }

        public async Task<List<ResponseManagerTaskModel>> GetManagerTaskByManagerId(Guid managerId)
        {
            var result = new List<ResponseManagerTaskModel>();
            var managerTask = await _dbContext.ManagerTask.Where(a => a.managerId == managerId && a.isDeleted == false).ToListAsync();
            if (managerTask == null)
            {
                return null;
            }
            else
            {
                foreach (var item in managerTask)
                {
                    var orderTmp = await _dbContext.Order.FindAsync(item.orderId);
                    var managerTmp = await _dbContext.User.FindAsync(item.managerId);
                    var createByTmp = await _dbContext.User.FindAsync(item.createById);
                    var tmp = new ResponseManagerTaskModel
                    {
                        createByName = createByTmp.fullName,
                        managerName = managerTmp.fullName,
                        orderName = orderTmp.name,
                        createdById = item.createById,
                        name = item.name,
                        startTime = item.startTime,
                        endTime = item.endTime,
                        status = item.status,
                        description = item.description,
                        isDeleted = item.isDeleted,
                    };
                    result.Add(tmp);
                }
                return result;
            }
        }

        public async Task<List<ResponseManagerTaskModel>> GetManagerTaskByFactory (Guid factoryId)
        {
            var result = new List<ResponseManagerTaskModel>();
            var managerTask = await _dbContext.ManagerTask.Where(a => a.createById == factoryId && a.isDeleted == false).ToListAsync();
            if (managerTask == null)
            {
                return null;
            }
            else
            {
                foreach (var item in managerTask)
                {
                    var orderTmp = await _dbContext.Order.FindAsync(item.orderId);
                    var managerTmp = await _dbContext.User.FindAsync(item.managerId);
                    var createByTmp = await _dbContext.User.FindAsync(item.createById);
                    var tmp = new ResponseManagerTaskModel
                    {
                        createByName = createByTmp.fullName,
                        managerName = managerTmp.fullName,
                        orderName = orderTmp.name,
                        createdById = item.createById,
                        name = item.name,
                        startTime = item.startTime,
                        endTime = item.endTime,
                        status = item.status,
                        description = item.description,
                        isDeleted = item.isDeleted,
                    };
                    result.Add(tmp);
                }
                return result;
            }
            
        }

        public async Task<ResultModel> UpdateManagerTaskStatus(Guid taskManagerId, TaskStatus status)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;
            var task = await _dbContext.ManagerTask.FindAsync(taskManagerId);
            if (task == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy Mannager Task!";
                return result;
            }
            else
            {
                try
                {
                    task.status = status;
                    await _dbContext.SaveChangesAsync();
                    result.Succeed = true;
                    result.Data = task.id;

                }

                catch (Exception ex)
                {
                    result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                }
                return result;
            }
            
        }

        public async Task<ResultModel> DeleteManagerTask(Guid taskManagerId)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false; 

            var check = await _dbContext.ManagerTask.FindAsync(taskManagerId);
            if (check == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy Mannager Task!";
                return result;
            }
            else
            {
                try
                {
                    check.isDeleted = true;
                    await _dbContext.SaveChangesAsync();
                    result.Succeed = true;
                    result.Data = check.id;

                }

                catch (Exception ex)
                {
                    result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                }
                return result;
            } 
        }        

        public async Task<ResultModel> AssignManagerTask(Guid taskManagerId, Guid groupId)
        {

            ResultModel result = new ResultModel();

            result.Succeed = false;
            var task = await _dbContext.ManagerTask.FindAsync(taskManagerId);
            if (task == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy Mannager Task!";
                return result;
            }
            else
            {
                var check = await _dbContext.Group.SingleOrDefaultAsync(x => x.id == groupId);
                if (check == null)
                {
                    result.Succeed = false;
                    result.ErrorMessage = "Nhóm không hợp lệ!";
                    return result;
                }
                else
                {
                    if (check.isDeleted == true)
                    {
                        result.Succeed = false;
                        result.ErrorMessage = "Nhóm đã xoá!";
                        return result;
                    }
                    else
                    {
                        try
                        {
                            task.groupId = groupId;
                            await _dbContext.SaveChangesAsync();
                            result.Succeed = true;
                            result.Data = task.id;
                        }
                        catch (Exception ex)
                        {
                            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                        }
                        return result;
                    }
                }
            }
        }

        /*
        public async Task<ResultModel> AssignManagerTask(AssignManagerTaskModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var check = await _dbContext.ManagerTaskGroup.SingleOrDefaultAsync(x => x.groupId == model.groupId && x.managerTaskId == model.taskManagerId);
            
            if (check != null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Tổ đã được gắn vào task!";
                return result;
            }

            var managerTaskGroup = new ManagerTaskGroup
            {
                groupId = model.groupId,
                managerTaskId = model.taskManagerId,
            };
            try
            {
                await _dbContext.ManagerTaskGroup.AddAsync(managerTaskGroup);
                await _dbContext.SaveChangesAsync();
                result.Succeed = true;
                result.Data = managerTaskGroup.id; 
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }
        
        public async Task<ResultModel> UnAssignManagerTask(AssignManagerTaskModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;
            var check = await _dbContext.ManagerTaskGroup.SingleOrDefaultAsync(x => x.groupId == model.groupId && x.managerTaskId == model.taskManagerId);

            if (check == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Tổ chưa được gắn vào task!";
                return result;
            }

            try
            {
                _dbContext.ManagerTaskGroup.Remove(check);
                await _dbContext.SaveChangesAsync();
                result.Succeed = true;
                result.Data = check.id;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }
        */

    }
}
