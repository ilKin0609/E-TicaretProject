using E_Ticaret_Project.Application.Abstracts.Services;
using Hangfire;
using System.Linq.Expressions;

namespace E_Ticaret_Project.Infrastructure.Services;

public class HangfireJobService: IBackgroundJobService
{
    public void Enqueue<T>(Expression<Action<T>> methodCall)
    {
        BackgroundJob.Enqueue(methodCall);
    }

    public void Schedule<T>(Expression<Action<T>> methodCall, TimeSpan delay)
    {
        BackgroundJob.Schedule(methodCall, delay);
    }
}
