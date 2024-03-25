using System;
using System.Linq;
using System.Threading.Tasks;
using Confab.Shared.Abstractions.Kernel;
using Microsoft.Extensions.DependencyInjection;

namespace Confab.Shared.Infrastructure.Kernel
{
    internal sealed class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public DomainEventDispatcher(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public async Task DispatchAsync(params IDomainEvent[] events)
        {
            if (events is null || !events.Any())
            {
                return;
            }

            using var scope = _serviceProvider.CreateScope();
            foreach (var @event in events)
            {
                var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(@event.GetType());
                var handlers = scope.ServiceProvider.GetServices(handlerType);

                var tasks = handlers.Select(x => (Task)handlerType
                    .GetMethod(nameof(IDomainEventHandler<IDomainEvent>.HandleAsync))
                    ?.Invoke(x, new[] { @event }));

                await Task.WhenAll(tasks);
            }
        }

        //TODO: doczytac czemu nie dziala :(
        //problem jest w tym (prawdopodobnie) bo, TDomainEvent to jeden, konkretny event (konkretna klasa)
        //uzywajac listy eventow mozemy miec rozne klasy dziedziczace po IDomainEvent, generyk nie zadziala
        public async Task DispatchAsync1<TDomainEvent>(params TDomainEvent[] events) where TDomainEvent : class, IDomainEvent
        {
            if (events is null || !events.Any())
            {
                return;
            }

            using var scope = _serviceProvider.CreateScope();
            foreach (var @event in events)
            {
                var handlers = scope.ServiceProvider.GetServices<IDomainEventHandler<TDomainEvent>>();
                var tasks = handlers.Select(x => x.HandleAsync(@event));

                await Task.WhenAll(tasks);
            }
        }
    }
}