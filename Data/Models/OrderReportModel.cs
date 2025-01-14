﻿using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class CreateOrderReportModel
    {
        public Guid? orderId { get; set; }
        public string title { get; set; } = null!;
        public string? content { get; set; } = null!;
        public ReportStatus? status { get; set; }
        public List<string>? resource { get; set; }
    }

    public class UpdateOrderReportModel
    {
        public Guid? id { get; set; }
        public string title { get; set; } = null!;
        public string? content { get; set; } = null!;
        public ReportStatus? status { get; set; }
        public List<string>? resource { get; set; }
    }

    public class OrderReportModel
    {
        public Guid id { get; set; }
        public Guid? orderId { get; set; }
        public OrderModel? order { get; set; }
        public string title { get; set; } = null!;
        public string? content { get; set; } = null!;
        public ReportStatus? status { get; set; }
        public DateTime createdDate { get; set; }
        public Guid reporterId { get; set; }
        public UserModel? reporter { get; set; }
        public List<string>? resource { get; set; }
    }
}
