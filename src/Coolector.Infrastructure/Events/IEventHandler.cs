using System.Threading.Tasks;
using Coolector.Core.Events;

namespace Coolector.Infrastructure.Events
{
    public interface IEventHandler<in T> where T : IEvent
    {
        Task HandleAsync(T @event);
    }
}