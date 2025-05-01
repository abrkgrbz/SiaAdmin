using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using SiaAdmin.Application.Interfaces.BackgroundJob;

namespace SiaAdmin.Infrastructure.Services
{
    public class HangfireBackgroundJobService:IBackgroundJobService
    {
        public string Enqueue<T>(Expression<Action<T>> methodCall) where T : class
        {
            return BackgroundJob.Enqueue(methodCall);
        }

        public string Schedule<T>(Expression<Action<T>> methodCall, TimeSpan delay) where T : class
        {
            return BackgroundJob.Schedule(methodCall, delay);
        }

        public string Schedule<T>(Expression<Action<T>> methodCall, DateTimeOffset enqueueAt) where T : class
        {
            return BackgroundJob.Schedule(methodCall, enqueueAt);
        }

        public bool Delete(string jobId)
        {
            return BackgroundJob.Delete(jobId);
        }

        public void RecurringJob<T>(string recurringJobId, Expression<Action<T>> methodCall, string cronExpression, TimeZoneInfo timeZone = null) where T : class
        {
            Hangfire.RecurringJob.AddOrUpdate<T>(
                recurringJobId,
                methodCall,
                cronExpression,
                timeZone: timeZone ?? TimeZoneInfo.Local);

        }

        public void RemoveRecurringJob(string recurringJobId)
        {
            RemoveRecurringJob(recurringJobId);
        }
    }
}
