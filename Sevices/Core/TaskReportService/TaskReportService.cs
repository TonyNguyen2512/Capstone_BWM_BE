﻿using Data.DataAccess;
using Data.Entities;
using Data.Enums;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Sevices.Core.NotificationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data.Models.TaskReportModel;

namespace Sevices.Core.ReportService
{
    public class TaskReportService : ITaskReportService
    {
        private readonly AppDbContext _dbContext;
        private readonly INotificationService _notificationService;

        public TaskReportService(AppDbContext dbContext, INotificationService notificationService)
        {
            _dbContext = dbContext;
            _notificationService = notificationService;
        }

        public ResultModel CreateAcceptanceReport(Guid reporterId, CreateAcceptanceReportModel model)
        {            
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var user = _dbContext.User.Include(r => r.Role).FirstOrDefault(i => i.Id == reporterId);
            if(user!.Role != null && user.Role.Name != "Leader")
            {
                result.Code = 50;
                result.Succeed = false;
                result.ErrorMessage = "Người dùng không phải trưởng nhóm!";
            }
            else
            {
                var leaderTask = _dbContext.LeaderTask
                .Where(x => x.id == model.leaderTaskId)
                .SingleOrDefault();

                if (leaderTask == null)
                {
                    result.Code = 51;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin công việc trưởng nhóm!";
                }
                else
                {
                    var checkReport = _dbContext.Report.Any(x => x.leaderTaskId == model.leaderTaskId && x.reportType == ReportType.AcceptanceReport);
                    var canSendReport = CanSendProgressTaskReport(leaderTask);
                    if (!canSendReport)
                    {
                        result.Code = 53;
                        result.Succeed = false;
                        result.ErrorMessage = "Không thể gửi báo cáo vào lúc này!";
                    }
                    else
                    {
                        if (checkReport == true)
                        {
                            result.Code = 52;
                            result.Succeed = false;
                            result.ErrorMessage = "Báo cáo nghiệm thu cho công việc này đã được thực hiện!";
                        }
                        else
                        {
                            var report = new Report
                            {
                                leaderTaskId = model.leaderTaskId,
                                reporterId = reporterId,
                                title = model.title,
                                content = model.content,
                                reportType = ReportType.AcceptanceReport,
                                createdDate = DateTime.Now,
                            };
                            var order = _dbContext.Order.Where(x => x.id == leaderTask.orderId).SingleOrDefault();

                            if (order != null)
                            {
                                order.acceptanceTime = DateTime.Now;
                            }

                            try
                            {
                                _dbContext.Report.Add(report);

                                if (model.resource != null)
                                {
                                    foreach (var resource in model.resource)
                                    {
                                        _dbContext.Resource.Add(new Resource
                                        {
                                            reportId = report.id,
                                            link = resource
                                        });
                                    }

                                    foreach (var resource in model.resource)
                                    {
                                        _dbContext.Resource.Add(new Resource
                                        {
                                            orderId = order!.id,
                                            link = resource
                                        });
                                    }
                                }
                                _dbContext.SaveChanges();

                                result.Succeed = true;
                                result.Data = report.id;
                            }

                            catch (Exception ex)
                            {
                                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                            }
                        }
                    }
                }
            }
            return result;
        }
        public ResultModel CreateProblemReport(Guid reporterId, CreateProblemReportModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var user = _dbContext.User.Include(r => r.Role).FirstOrDefault(i => i.Id == reporterId);
            if (user!.Role != null && user.Role.Name != "Leader")
            {
                result.Code = 50;
                result.Succeed = false;
                result.ErrorMessage = "Người dùng không phải trưởng nhóm!";
            }
            else
            {
                var leaderTask = _dbContext.LeaderTask
                .Where(x => x.id == model.leaderTaskId)
                .SingleOrDefault();

                if (leaderTask == null)
                {
                    result.Code = 51;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin công việc trưởng nhóm!";
                }

                else
                {
                    var canSendReport = CanSendProblemTaskReport(leaderTask);

                    if (!canSendReport)
                    {
                        result.Code = 53;
                        result.Succeed = false;
                        result.ErrorMessage = "Không thể gửi báo cáo vào lúc này!";
                    }
                    else
                    {
                        var report = new Report
                        {
                            leaderTaskId = model.leaderTaskId,
                            reporterId = reporterId,
                            title = model.title,
                            content = model.content,
                            reportType = ReportType.ProgressReport,
                            createdDate = DateTime.Now,
                        };

                        try
                        {
                            _dbContext.Report.Add(report);

                            if (model.resource != null)
                            {
                                foreach (var resource in model.resource)
                                {
                                    _dbContext.Resource.Add(new Resource
                                    {
                                        reportId = report.id,
                                        link = resource
                                    });
                                }
                            }
                            _dbContext.SaveChanges();

                            result.Succeed = true;
                            result.Data = report.id;
                        }

                        catch (Exception ex)
                        {
                            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                        }
                    }
                }


            }
            return result;
        }
        public ResultModel CreateProgressReport(Guid reporterId, CreateProgressReportModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var user = _dbContext.User.Include(r => r.Role).FirstOrDefault(i => i.Id == reporterId);
            if (user!.Role != null && user.Role.Name != "Leader")
            {
                result.Code = 50;
                result.Succeed = false;
                result.ErrorMessage = "Người dùng không phải trưởng nhóm!";
            }
            else
            {
                var leaderTask = _dbContext.LeaderTask
                .Where(x => x.id == model.leaderTaskId)
                .SingleOrDefault();

                if (leaderTask == null)
                {
                    result.Code = 51;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin công việc trưởng nhóm!";
                }
                else
                {

                    var canSendReport = CanSendProgressTaskReport(leaderTask);


                    if (!canSendReport)
                    {
                        result.Code = 53;
                        result.Succeed = false;
                        result.ErrorMessage = "Không thể gửi báo cáo vào lúc này!";
                    }
                    else
                    {
                        var report = new Report
                        {
                            reporterId = reporterId,
                            leaderTaskId = model.leaderTaskId,
                            title = model.title,
                            content = model.content,
                            reportType = ReportType.ProgressReport,
                            reportStatus = model.reportStatus,
                            createdDate = DateTime.Now,
                        };

                        try
                        {
                            _dbContext.Report.Add(report);

                            if (model.resource != null)
                            {
                                foreach (var resource in model.resource)
                                {
                                    _dbContext.Resource.Add(new Resource
                                    {
                                        reportId = report.id,
                                        link = resource
                                    });
                                }
                            }

                            _dbContext.Report.Add(report);
                            _dbContext.SaveChanges();
                            result.Succeed = true;
                            result.Data = report.id;
                        }
                        catch (Exception ex)
                        {
                            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                        }
                    }
                }
            }
            return result;
        }
        public ResultModel SendProblemResponse(SendProblemResponseModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var report = _dbContext.Report.Include(x => x.LeaderTask)
                .Where(x => x.id == model.reportId).SingleOrDefault();

            if (report == null)
            {
                result.Code = 54;
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin báo cáo!";
            }
            else
            {
                try
                {
                    report.responseContent = model.responseContent;
                    _dbContext.SaveChanges();
                    result.Succeed = true;
                    result.Data = report.id;
                }
                catch (Exception ex)
                {
                    result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                }
            }
            return result;
        }
        public ResultModel SendProgressResponse(SendProgressResponseModel model)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;

            var report = _dbContext.Report.Include(x => x.LeaderTask)
                .Where(x => x.id == model.reportId).SingleOrDefault();

            if (report == null)
            {
                result.Code = 54;
                result.Succeed = false;
                result.ErrorMessage = "Không tìm thấy thông tin báo cáo!";
            }
            else
            {

                if (report.reportStatus == ReportStatus.Complete)
                {
                    result.Code = 55;
                    result.Succeed = false;
                    result.ErrorMessage = "Báo cáo này đã hoàn thành!";
                }
                else
                {
                    var leaderTask = _dbContext.LeaderTask
                    .Find(report.leaderTaskId);

                    try
                    {
                        report.reportStatus = model.reportStatus;
                        report.responseContent = model.responseContent;

                        if (leaderTask != null && model.reportStatus == ReportStatus.Complete)
                        {
                            leaderTask.completedTime = DateTime.Now;
                            leaderTask.status = ETaskStatus.Completed;
                        }
                        else if (leaderTask != null && model.reportStatus == ReportStatus.Uncomplete)
                        {
                            leaderTask.status = ETaskStatus.NotAchived;
                        }
                        _dbContext.SaveChanges();
                        result.Succeed = true;
                        result.Data = report.id;
                    }

                    catch (Exception ex)
                    {
                        result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    }
                }
            }
            return result;
        }       
        public ResultModel GetById(Guid id)
        {
            ResultModel result = new ResultModel();
            result.Succeed = false;
            try
            {
                var report = _dbContext.Report
               .Include(x => x.LeaderTask).Include(x => x.Resources)
               .Where(x => x.id == id).SingleOrDefault();

                if (report == null)
                {
                    result.Code = 54;
                    result.Succeed = false;
                    result.ErrorMessage = "Không tìm thấy thông tin báo cáo!";
                }
                else
                {
                    if (report.reportType == ReportType.ProgressReport)
                    {
                        var reviewer = report.LeaderTask.CreateBy;
                        var reporter = report.Reporter;

                        var taskReport = new TaskReportModel
                        {
                            id = report.id,
                            leaderTaskId = report.LeaderTask.id,
                            title = report.title,
                            content = report.content,
                            createdDate = report.createdDate,
                            reportStatus = report.reportStatus,
                            responseContent = report.responseContent,
                            reporterId = report.reporterId,
                            responderId = report.LeaderTask.createById,
                            resource = report.Resources.Select(x => x.link).ToList()
                        };
                        result.Data = taskReport;
                        result.Succeed = true;
                    }
                    else
                    {
                        var reviewer = report.LeaderTask.CreateBy;
                        var reporter = report.Reporter;

                        var taskReport = new TaskReportModel
                        {
                            id = report.id,
                            leaderTaskId = report.leaderTaskId,
                            title = report.title,
                            content = report.content,
                            createdDate = report.createdDate,
                            responseContent = report.responseContent,
                            reporterId = report.reporterId,
                            responderId = report.LeaderTask.createById,
                            resource = report.Resources.Select(x => x.link).ToList()
                        };
                        result.Data = taskReport;
                        result.Succeed = true;
                    }                    
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return result;
        }            
        public ResultModel GetProblemReportByLeaderTaskId(Guid leaderTaskId, string? search, int pageIndex, int pageSize)
        {
            var result = new ResultModel();
            result.Succeed = false;

            var listTaskReport = _dbContext.Report
                .Include(x => x.LeaderTask).Include(x => x.Resources)
                .Where(x => x.leaderTaskId == leaderTaskId && x.reportType == ReportType.ProblemReport).OrderByDescending(x => x.createdDate)
                .ToList();

            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    listTaskReport = listTaskReport.Where(x => x.title.Contains(search)).ToList();
                }

                var listLeaderTaskPaging = listTaskReport.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = new List<TaskReportModel>();
                foreach (var item in listLeaderTaskPaging)
                {
                    var tmp = new TaskReportModel
                    {
                        id = item.id,
                        title = item.title,
                        content = item.content,
                        createdDate = item.createdDate,
                        responseContent = item.responseContent,
                        reporterId = item.reporterId,
                        responderId = item.LeaderTask.createById,
                        resource = item.Resources.Select(x => x.link).ToList()
                    };
                    list.Add(tmp);
                }

                result.Data = new PagingModel()
                {
                    Data = list,
                    Total = listTaskReport.Count
                };
                result.Succeed = true;

            }

            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }

            return result;
        }
        public ResultModel GetProgressReportByLeaderTaskId(Guid leaderTaskId, string? search, int pageIndex, int pageSize)
        {
            var result = new ResultModel();
            result.Succeed = false;

            var listTaskReport = _dbContext.Report
                .Include(x => x.LeaderTask).Include(x => x.Resources)
                .Where(x => x.leaderTaskId == leaderTaskId && x.reportType == ReportType.ProgressReport).OrderByDescending(x => x.createdDate).ToList();

            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    listTaskReport = listTaskReport.Where(x => x.title.Contains(search)).ToList();
                }

                var listLeaderTaskPaging = listTaskReport.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var list = new List<TaskReportModel>();
                foreach (var item in listLeaderTaskPaging)
                {
                    var tmp = new TaskReportModel
                    {
                        id = item.id,
                        title = item.title,
                        content = item.content,
                        createdDate = item.createdDate,
                        reportStatus = item.reportStatus,
                        responseContent = item.responseContent,
                        reporterId = item.reporterId,
                        responderId = item.LeaderTask.createById,
                        resource = item.Resources.Select(x => x.link).ToList()
                    };
                    list.Add(tmp);
                }

                result.Data = new PagingModel()
                {
                    Data = list,
                    Total = listTaskReport.Count
                };
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }

            return result;
        }
        #region Validate
        private bool CanSendProgressTaskReport(LeaderTask leaderTask)
        {
            var now = DateTime.Now;
            return now >= leaderTask.endTime;
        }
        private bool CanSendProblemTaskReport(LeaderTask leaderTask)
        {
            var now = DateTime.Now;
            return now >= leaderTask.startTime && now <= leaderTask.endTime;
        }
        #endregion
    }
}
