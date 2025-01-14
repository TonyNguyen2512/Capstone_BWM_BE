﻿using Data.Entities;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class CreateProgressReportModel
    {
        public Guid? leaderTaskId { get; set; }
        public string title { get; set; } = null!;
        public string? content { get; set; } = null!;
        public int itemFailed { get; set; }
        public List<string>? resource { get; set; }
    }

    public class CreateProblemReportModel
    {
        public Guid? leaderTaskId { get; set; }
        public string title { get; set; } = null!;
        public string? content { get; set; } = null!;
        public List<string>? resource { get; set; }
        public ESupplyStatus supplyStatus { get; set; }
        public List<MaterialAmount> listSupply { get; set; } = new();
    }

    public class CreateAcceptanceReportModel
    {
        public Guid? acceptanceTaskId { get; set; }
        public string title { get; set; } = null!;
        public string? content { get; set; } = null!;
        public List<string>? resource { get; set; }
    }

    public class UpdateProblemTaskReportModel
    {
        public Guid id { get; set; }
        public string title { get; set; } = null!;
        public string? content { get; set; } = null!;
        public List<string>? resource { get; set; }
        public ESupplyStatus supplyStatus { get; set; }
        public List<MaterialAmount> listSupply { get; set; } = new();
    }

    public class UpdateTaskReportModel
    {
        public Guid id { get; set; }
        public string title { get; set; } = null!;
        public string? content { get; set; } = null!;
        public List<string>? resource { get; set; }
    }

    public class TaskReportModel
    {
        public Guid? id { get; set; }
        public Guid? leaderTaskId { get; set; }
        public string? leaderTaskName { get; set; } = null!;
        public Guid? reporterId { get; set; }
        public string? reporterName { get; set; } = null!;
        public Guid? responderId { get; set; }
        public string? responderName { get; set; } = null!;
        public ReportType? reportType { get; set; }
        public string? title { get; set; } = null!;
        public string? content { get; set; } 
        public ReportStatus? status { get; set; }
        public int? itemQuantity { get; set; }
        public int? itemFailed { get; set; }    
        public List<string>? resource { get; set; }
        public DateTime? createdDate { get; set; }
        public string? responseContent { get; set; }
        public List<SupplyModel>? listSupply { get; set; } = new();
    }

    public class SendProblemReportFeedbackModel
    {
        public Guid reportId { get; set; }
        public ReportStatus status { get; set; }
        public string responseContent { get; set; } = null!;
    }
    public class SendProgressReportFeedbackModel
    {
        public Guid reportId { get; set; }
        public ReportStatus status { get; set; }
        public string responseContent { get; set; } = null!;
    }

    public class MaterialAmount
    {
        public Guid materialId { get; set; }
        public int amount { get; set; }
    }

    public class TaskReportViewModel
    {
        public Guid? id { get; set; }
        public Guid? leaderTaskId { get; set; }
        public string? leaderTaskName { get; set; } = null!;
        public Guid? reporterId { get; set; }
        public string? reporterName { get; set; } = null!;
        public Guid? responderId { get; set; }
        public string? responderName { get; set; } = null!;
        public ReportType? reportType { get; set; }
        public string? title { get; set; } = null!;
        public string? content { get; set; }
        public ReportStatus? status { get; set; }
        public int? itemQuantity { get; set; }
        public int? itemFailed { get; set; }
        public List<string>? resource { get; set; }
        public DateTime? createdDate { get; set; }
        public string? responseContent { get; set; }
        public List<SupplyModel>? supply { get; set; }
    }
}
