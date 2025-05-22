using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.DTOs.NotificationDetailDto
{
    public class NotificationDetailDto
    {
        public int id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string project { get; set; }
        public string sendTime { get; set; }
        public int status { get; set; }
        public int? recipientCount { get; set; }
        public int? successCount { get; set; }
        public int? failedCount { get; set; }
        public string updateTime { get; set; }
        public string sender { get; set; }
        public string provider { get; set; } 
        public ErrorDetailDto errorDetails { get; set; }
        public string payload { get; set; }
    }

    public class ErrorDetailDto
    {
        public string code { get; set; }
        public string message { get; set; }
    }
}
