using System.Threading.Tasks;
using Coolector.Core.Events;

namespace Coolector.Infrastructure.Events
{
    public interface IEventDispatcher
    {
        Task DispatchAsync<T>(params T[] events) where T : IEvent;
    }
}