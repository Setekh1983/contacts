using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alex.DddBasics
{
  internal abstract class DomainEventHandlerWrapper
  {
    public abstract Task Handle(IDomainEvent domainEvent, FactoryFunction factory);
  }

  internal class DomainEventHandlerWrapperImpl<TDomainEvent> : DomainEventHandlerWrapper
    where TDomainEvent : IDomainEvent
  {
    public override async Task Handle(IDomainEvent domainEvent, FactoryFunction factory)
    {
      var domainEventHandlers = (IEnumerable<IDomainEventHandler<TDomainEvent>>)factory(typeof(IEnumerable<IDomainEventHandler<TDomainEvent>>));

      foreach (IDomainEventHandler<TDomainEvent> handler in domainEventHandlers)
      {
        await handler.Handle((TDomainEvent)domainEvent);
      }
    }
  }
}