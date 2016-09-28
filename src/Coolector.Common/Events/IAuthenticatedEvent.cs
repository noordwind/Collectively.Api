namespace Coolector.Common.Events
{
    public interface IAuthenticatedEvent : IEvent
    {
        string UserId { get; set; }
    }
}