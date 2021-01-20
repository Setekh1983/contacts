using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alex.DddBasics.Test.Domain
{
  class DomainEventHandlerStub
  {
    static readonly Type MarriedHandler = typeof(IEnumerable<IDomainEventHandler<CitizenMarriedEvent>>);
    static readonly Type MovedHandler = typeof(IEnumerable<IDomainEventHandler<CitizenMovedEvent>>);

    public IList<IDomainEvent> HandledEvents = new List<IDomainEvent>();

    public class CitizenMarriedDomainEventHandler1Stub : IDomainEventHandler<CitizenMarriedEvent>
    {
      public IList<IDomainEvent> HandledEvents { get; }
      public CitizenMarriedDomainEventHandler1Stub(IList<IDomainEvent> handledEvents) => this.HandledEvents = handledEvents;
      public Task Handle(CitizenMarriedEvent domainEvent)
      {
        this.HandledEvents.Add(domainEvent);
        return Task.CompletedTask;
      }
    }
    public class CitizenMarriedDomainEventHandler2Stub : IDomainEventHandler<CitizenMarriedEvent>
    {
      public IList<IDomainEvent> HandledEvents { get; }
      public CitizenMarriedDomainEventHandler2Stub(IList<IDomainEvent> handledEvents) => this.HandledEvents = handledEvents;
      public Task Handle(CitizenMarriedEvent domainEvent)
      {
        this.HandledEvents.Add(domainEvent);
        return Task.CompletedTask;
      }
    }

    public class CitizenMovedDomainEventHandler1Stub : IDomainEventHandler<CitizenMovedEvent>
    {
      public IList<IDomainEvent> HandledEvents { get; }
      public CitizenMovedDomainEventHandler1Stub(IList<IDomainEvent> handledEvents) => this.HandledEvents = handledEvents;
      public Task Handle(CitizenMovedEvent domainEvent)
      {
        this.HandledEvents.Add(domainEvent);
        return Task.CompletedTask;
      }
    }

    public object CreateHandler(Type type)
    {
      if (MarriedHandler == type)
      {
        return new List<IDomainEventHandler<CitizenMarriedEvent>>() {
          new CitizenMarriedDomainEventHandler1Stub(this.HandledEvents),
          new CitizenMarriedDomainEventHandler1Stub(this.HandledEvents)
        };
      }
      else if (MovedHandler == type)
      {
        return new List<IDomainEventHandler<CitizenMovedEvent>>() {
          new CitizenMovedDomainEventHandler1Stub(this.HandledEvents)
        };
      }

      return new List<IDomainEventHandler<CitizenDiedEvent>>();
    }
  }
}
