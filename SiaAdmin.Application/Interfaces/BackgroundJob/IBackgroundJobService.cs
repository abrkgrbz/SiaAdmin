using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Interfaces.BackgroundJob
{
    public interface IBackgroundJobService
    {
        string Enqueue<T>(Expression<Action<T>> methodCall) where T : class;
        string Schedule<T>(Expression<Action<T>> methodCall, TimeSpan delay) where T : class;
        string Schedule<T>(Expression<Action<T>> methodCall, DateTimeOffset enqueueAt) where T : class;
        bool Delete(string jobId);
        void RecurringJob<T>(string recurringJobId, Expression<Action<T>> methodCall, string cronExpression, TimeZoneInfo timeZone = null) where T : class;
        void RemoveRecurringJob(string recurringJobId);
    }
}
