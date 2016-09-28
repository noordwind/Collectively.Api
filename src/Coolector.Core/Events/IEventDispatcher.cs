using System.Threading.Tasks;
using Coolector.Common.Events;

namespace Coolector.Core.Events
{
    public interface IEventDispatcher
    {
        Task DispatchAsync<T>(params T[] events) where T : IEvent;
    }
}