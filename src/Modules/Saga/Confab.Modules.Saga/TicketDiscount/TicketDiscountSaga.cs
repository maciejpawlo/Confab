using Confab.Shared.Abstractions.Messaging;
using Chronicle;
using Confab.Modules.Saga.Messages;
using System.Threading.Tasks;

namespace Confab.Modules.Saga.TicketDiscount
{
	internal class TicketDiscountSaga : Saga<TicketDiscountSaga.SagaData>,
        ISagaStartAction<TicketPurchased>
	{
		public const int TICKET_TRESHOLD = 3;
        private readonly IMessageBroker _messageBroker;

        public override SagaId ResolveId(object message, ISagaContext context)
            => message switch
            {
                TicketPurchased m => m.UserId.ToString(),
                _ => base.ResolveId(message, context)
            };

        public TicketDiscountSaga(IMessageBroker messageBroker)
        {
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(TicketPurchased message, ISagaContext context)
        {
            Data.TicketCount++;
            if (Data.TicketCount < TICKET_TRESHOLD)
                return;

            await _messageBroker.PublishAsync(new AddTicketDiscount(message.UserId, 30));
            await CompleteAsync();
        }

        public Task CompensateAsync(TicketPurchased message, ISagaContext context)
            => Task.CompletedTask;

        internal class SagaData
        {
            public int TicketCount { get; set; }
        }
    }
}

