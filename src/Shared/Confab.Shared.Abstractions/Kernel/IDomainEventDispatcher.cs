using System.Threading.Tasks;

namespace Confab.Shared.Abstractions.Kernel
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(params IDomainEvent[] events);
        Task DispatchAsync1<TDomainEvent>(params TDomainEvent[] events) where TDomainEvent : class, IDomainEvent;
    }
}