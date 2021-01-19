using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alex.DddBasics.EventStoreDB.Test.Domain
{
  class DomainEventHandlerStub
  {
    static readonly Type MarriedHandler = typeof(IEnumerable<IDomainEventHandler<CitizenMarriedEvent>>);
    
    public IList<IDomainEvent> HandledEvents = new List<IDomainEvent>();

    public class CitizenMarriedDomainEventHandler1Stub : IDomainEventHandler<CitizenMarriedEvent>
    {
      public IList<IDomainEvent> HandledEvents { get; }
      public CitizenMarriedDomainEventHandler1Stub(IList<IDomainEvent> handledEvents) => this.HandledEvents = handledEvents;
      public async Task Handle(CitizenMarriedEvent domainEvent) => HandledEvents.Add(domainEvent);
    }
    public object CreateHandler(Type type)
    {
      if (MarriedHandler == type)
      {
        return new List<IDomainEventHandler<CitizenMarriedEvent>>() {
          new CitizenMarriedDomainEventHandler1Stub(this.HandledEvents)
        };
      }
      return null;
    }
  }
}
