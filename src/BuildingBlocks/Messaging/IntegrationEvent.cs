
namespace Messaging
{
    public class IntegrationEvent
    {
        public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;
    }
}
