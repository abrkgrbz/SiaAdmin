using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.DTOs.NotificationDTO
{
    public class NotificationResult
    {
        public bool Success { get; set; }
        public int TotalCount { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public string Payload { get; set; }
    }
}
