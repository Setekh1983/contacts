using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Alex.DddBasics
{
  public abstract class AggregateRoot : Entity
  {
    public AggregateRoot()
      : base() => this.Events = new List<IDomainEvent>();

    public AggregateRoot(Guid id)
    : base(id) => this.Events = new List<IDomainEvent>();

    List<IDomainEvent> Events { get; }

    public IReadOnlyCollection<IDomainEvent> GetChanges() => new ReadOnlyCollection<IDomainEvent>(this.Events);
    public void ClearEvents() => this.Events.Clear();

    protected void ApplyEvent(IDomainEvent domainEvent)
    {
      _ = domainEvent ?? throw new ArgumentNullException(nameof(domainEvent));

      dynamic exposed = new ExposedObject(this);
      _ = exposed.Apply((dynamic)domainEvent);

      this.Events.Add(domainEvent);
    }
  }
}
