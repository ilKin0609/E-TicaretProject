using System.Linq.Expressions;

namespace E_Ticaret_Project.Application.Abstracts.Services;

public interface IBackgroundJobService
{
    void Enqueue<T>(Expression<Action<T>> methodCall);
    void Schedule<T>(Expression<Action<T>> methodCall, TimeSpan delay);
}
