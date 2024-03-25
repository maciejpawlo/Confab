using System;
using Confab.Shared.Abstractions.Events;

namespace Confab.Modules.Saga.Messages
{
	internal record TicketPurchased(Guid TicketId, Guid ConferenceId, Guid UserId) : IEvent;
}

