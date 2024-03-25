using System;
using Confab.Shared.Abstractions.Commands;
namespace Confab.Modules.Saga.Messages
{
	internal record AddTicketDiscount(Guid UserId, decimal Percentage) : ICommand;
}

