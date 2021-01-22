using EventStore.Client;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Alex.DddBasics.EventStoreDB
{
  public class Repository<TAggregate> : IRepository<TAggregate> where TAggregate : AggregateRoot
  {
    const BindingFlags CreationBindingFlags =
      BindingFlags.Instance | BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.NonPublic;

    public Repository(EventStoreClient eventStoreClient, EventTypeRegistry eventTypeMap, 
      IDomainEventDispatcher domainEventDispatcher)
    {
      _ = eventStoreClient ?? throw new ArgumentNullException(nameof(eventStoreClient));
      _ = eventTypeMap ?? throw new ArgumentNullException(nameof(eventTypeMap));
      _ = domainEventDispatcher ?? throw new ArgumentNullException(nameof(domainEventDispatcher));

      this.DomainEventDispatcher = domainEventDispatcher;
      this.EventStoreClient = eventStoreClient;
      this.AggregateTypeName = typeof(TAggregate).Name.ToLower();
      this.AggregateType = typeof(TAggregate);
      this.EventTypeMap = eventTypeMap;
    }

    IDomainEventDispatcher DomainEventDispatcher { get; }
    EventStoreClient EventStoreClient { get; }
    string AggregateTypeName { get; }
    Type AggregateType { get; }
    EventTypeRegistry EventTypeMap { get; }

    string GetStreamName(Guid id) => $"{this.AggregateTypeName}-{id}";

    public async Task SaveAsync(TAggregate aggregate)
    {
      IPersistableAggregate persistable = aggregate;

      StreamRevision revision = persistable.OriginatingVersion == -1
        ? StreamRevision.None
        : StreamRevision.FromInt64(persistable.OriginatingVersion);

      IEnumerable<IDomainEvent> events = persistable.GetChanges();
      List<EventData> eventData = PrepareEventData(events);

      IWriteResult result = await this.EventStoreClient.AppendToStreamAsync(
        this.GetStreamName(persistable.Id), revision, eventData);

      persistable.ChangesSaved(result.NextExpectedStreamRevision.ToInt64());

      await this.DomainEventDispatcher.Dispatch(events);
    }

    public async Task<TAggregate> LoadAsync(Guid id)
    {
      await using var result = this.EventStoreClient.ReadStreamAsync(
        Direction.Forwards, this.GetStreamName(id), StreamPosition.Start);

      if (await result.ReadState == ReadState.StreamNotFound)
      {
        return null;
      }
      var readEventTask = this.GetEvents(result);
      var aggregate = this.CreateInstance(id);

      IPersistableAggregate persistable = aggregate;

      (var domainEvents, var version) = await readEventTask;
      persistable.LoadFromEvents(domainEvents, version);

      return aggregate;

    }

    TAggregate CreateInstance(Guid id)
    {
      return (TAggregate)Activator.CreateInstance(this.AggregateType,
              CreationBindingFlags, null, new object[] { id }, null);
    }

    async Task<(List<IDomainEvent>, long latestEventId)> GetEvents(EventStoreClient.ReadStreamResult streamResult)
    {
      var domainEvents = new List<IDomainEvent>();
      StreamPosition latestEventNumber = StreamPosition.Start;

      await foreach (var anEvent in streamResult)
      {
        Type domainEventType = this.EventTypeMap.GetType(anEvent.Event.EventType);
        var domainEvent = (IDomainEvent)JsonSerializer.Deserialize(anEvent.Event.Data.Span, domainEventType);

        domainEvents.Add(domainEvent);
        latestEventNumber = anEvent.Event.EventNumber;
      }
      return (domainEvents, latestEventNumber.ToInt64());
    }

    static List<EventData> PrepareEventData(IEnumerable<IDomainEvent> events)
    {
      var eventData = new List<EventData>();

      foreach (IDomainEvent domainEvent in events)
      {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(domainEvent, domainEvent.GetType());

        var data = new EventData(Uuid.NewUuid(), domainEvent.GetType().Name.ToLower(),
          new ReadOnlyMemory<byte>(bytes));

        eventData.Add(data);
      }
      return eventData;
    }
  }
}
