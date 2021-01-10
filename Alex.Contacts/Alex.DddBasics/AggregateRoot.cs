using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Alex.DddBasics
{
  public abstract class AggregateRoot : Entity, IPersistableAggregate
  {
    public AggregateRoot()
      : this(Guid.NewGuid()) => this.OriginatingVersion = -1;

    public AggregateRoot(Guid id)
    : base(id)
    {
      this.Events = new List<IDomainEvent>();
      this._ExposedObject = new ExposedObject(this);
    }
    private readonly dynamic _ExposedObject;

    List<IDomainEvent> Events { get; }
    long OriginatingVersion { get; set; }
    long IPersistableAggregate.OriginatingVersion => this.OriginatingVersion;

    protected void ApplyEvent(IDomainEvent domainEvent) => this.ApplyEvent(domainEvent, true);

    private void ApplyEvent(IDomainEvent domainEvent, bool isNew)
    {
      _ = domainEvent ?? throw new ArgumentNullException(nameof(domainEvent));

      //dynamic exposed = new ExposedObject(this);
      _ = this._ExposedObject.Apply((dynamic)domainEvent);

      if (isNew)
      {
        this.Events.Add(domainEvent);
      }
    }

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
