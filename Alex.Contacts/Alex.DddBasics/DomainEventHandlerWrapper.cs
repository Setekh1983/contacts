using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alex.DddBasics
{
  abstract class DomainEventHandlerWrapper
  {
    public abstract Task Handle(IDomainEvent domainEvent, FactoryFunction factory);
  }

  class DomainEventHandlerWrapperImpl<TDomainEvent> : DomainEventHandlerWrapper
    where TDomainEvent : IDomainEvent
  {
    public override async Task Handle(IDomainEvent domainEvent, FactoryFunction factory)
    {
      var domainEventHandlers = (IEnumerable<IDomainEventHandler<TDomainEvent>>?)factory(typeof(IEnumerable<IDomainEventHandler<TDomainEvent>>));

      if (domainEventHandlers is not null)
      {
        foreach (IDomainEventHandler<TDomainEvent> handler in domainEventHandlers)
        {
          await handler.Handle((TDomainEvent)domainEvent);
        }
      }
    }
  }
}