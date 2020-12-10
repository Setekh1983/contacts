using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Alex.DddBasics
{
  public abstract class AggregateRoot : Entity, IPersistableAggregate
  {
    public AggregateRoot()
      : base() => this.Events = new List<IDomainEvent>();

    public AggregateRoot(Guid id)
    : base(id) => this.Events = new List<IDomainEvent>();

    List<IDomainEvent> Events { get; }
    long OriginatingVersion { get; set; }

    protected void ApplyEvent(IDomainEvent domainEvent)
    {
      _ = domainEvent ?? throw new ArgumentNullException(nameof(domainEvent));

      dynamic exposed = new ExposedObject(this);
      _ = exposed.Apply((dynamic)domainEvent);

      this.Events.Add(domainEvent);
    }

    long IPersistableAggregate.OriginatingVersion => this.OriginatingVersion;

    public IEnumerable<IDomainEvent> GetChanges() => new ReadOnlyCollection<IDomainEvent>(this.Events);
    void IPersistableAggregate.ChangesSaved(long newVersion)
    {
      if (newVersion <= this.OriginatingVersion)
      {
        throw new InvalidOperationException(Properties.Resources.NewVersionMustBeGreaterThenPreviousVersion);
      }
      this.Events.Clear();
      this.OriginatingVersion = newVersion;
    }

  }
}
