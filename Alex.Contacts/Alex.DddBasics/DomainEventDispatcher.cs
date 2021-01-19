using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alex.DddBasics
{
  public delegate object? FactoryFunction(Type type);

  public class DomainEventDispatcher : IDomainEventDispatcher
  {
    static readonly Type AbstractHandlerWrapper = typeof(DomainEventHandlerWrapperImpl<>);

    readonly FactoryFunction _HandlerFactory;
    readonly ConcurrentDictionary<Type, DomainEventHandlerWrapper> _DomainEventHandlers;

    public DomainEventDispatcher(FactoryFunction handlerfactory)
    {
      _ = handlerfactory ?? throw new ArgumentNullException(nameof(handlerfactory));

      this._DomainEventHandlers = new();
      this._HandlerFactory = handlerfactory;
    }

    public async Task Dispatch<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : IDomainEvent
    {
      _ = domainEvent ?? throw new ArgumentNullException(nameof(domainEvent));

      var domainEventType = domainEvent.GetType();
      var handler = _DomainEventHandlers.GetOrAdd(domainEventType, CreateInstance);

      await handler.Handle(domainEvent, _HandlerFactory);
    }

    public async Task Dispatch<TDomainEvent>(IEnumerable<TDomainEvent> domainEvents) where TDomainEvent : IDomainEvent
    {
      _ = domainEvents ?? throw new ArgumentNullException(nameof(domainEvents));

      foreach (IDomainEvent domainEvent in domainEvents)
      {
        await this.Dispatch(domainEvent);
      }
    }

    static DomainEventHandlerWrapper CreateInstance(Type domainEventType)
    {
      var genericHandlerType = AbstractHandlerWrapper.MakeGenericType(domainEventType);

      return (DomainEventHandlerWrapper)Activator.CreateInstance(genericHandlerType);
    }
  }
}
