using System;
using System.Threading.Tasks;
using Chronicle;
using Confab.Modules.Saga.Messages;
using Confab.Shared.Abstractions.Events;

namespace Confab.Modules.Saga.TicketDiscount
{
	internal class Handler : IEventHandler<TicketPurchased>
	{
        private readonly ISagaCoordinator _coordinator;

        public Handler(ISagaCoordinator coordinator)
		{
            _coordinator = coordinator;
        }

        public Task HandleAsync(TicketPurchased @event) => _coordinator.ProcessAsync(@event, SagaContext.Empty);
    }
}

