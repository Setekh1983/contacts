using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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

    protected void ApplyEvent(IDomainEvent domainEvent) => this.ApplyEvent(domainEvent, true);
    private void ApplyEvent(IDomainEvent domainEvent, bool isNew)
    {
      _ = domainEvent ?? throw new ArgumentNullException(nameof(domainEvent));

      dynamic exposed = new ExposedObject(this);
      _ = exposed.Apply((dynamic)domainEvent);

      if (isNew)
      {
        this.Events.Add(domainEvent);
      }
    }

    long IPersistableAggregate.OriginatingVersion => this.OriginatingVersion;

    public IEnumerable<IDomainEvent> GetChanges() => new ReadOnlyCollection<IDomainEvent>(this.Events);
    void IPersistableAggregate.ChangesSaved(long newVersion)
    {
      if (newVersion <= this.OriginatingVersion)
      {
        throw new ArgumentException(Properties.Resources.NewVersionMustBeGreaterThenPreviousVersion);
      }
      this.Events.Clear();
      this.OriginatingVersion = newVersion;
    }

    void IPersistableAggregate.LoadFromEvents(IEnumerable<IDomainEvent> events, long version)
    {
      _ = events ?? throw new ArgumentNullException(nameof(events));

      if (version < 0)
      {
        throw new ArgumentException(Properties.Resources.VersionMustBeGreaterThenZero);
      }
      if (!events.Any())
      {
        throw new ArgumentException(Properties.Resources.EventsMustNotBeEmpty);
      }
      foreach (IDomainEvent currentEvent in events)
      {
        this.ApplyEvent(currentEvent, false);
      }
      this.OriginatingVersion = version;
    }
  }
}
