using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Handlers
{
    public class ProductCreatedIntegrationEventHandler : IRequestHandler<ProductCreatedIntegrationEvent, bool>
    {
        public Task<bool> Handle(ProductCreatedIntegrationEvent request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
