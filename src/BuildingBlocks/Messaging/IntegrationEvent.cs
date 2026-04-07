
using MediatR;

namespace Messaging
{
    public class IntegrationEvent:IRequest<bool>
    {
        public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;
    }
}
