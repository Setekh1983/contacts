using System;

namespace Alex.DddBasics
{
  public abstract class DomainEvent : IDomainEvent
  {
    protected DomainEvent() => this.EventId = Guid.NewGuid();

    public Guid EventId { get; }
  }
}
