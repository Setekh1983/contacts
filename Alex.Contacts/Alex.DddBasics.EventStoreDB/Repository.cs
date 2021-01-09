using EventStore.Client;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Alex.DddBasics.EventStoreDB
{
  public class Repository<TAggregate> : IRepository<TAggregate> where TAggregate : AggregateRoot, new()
  {
    public Repository(EventStoreClient eventStoreClient, EventTypeRegistry eventTypeMap)
    {
      _ = eventStoreClient ?? throw new ArgumentNullException(nameof(eventStoreClient));
      _ = eventTypeMap ?? throw new ArgumentNullException(nameof(eventTypeMap));

      this.EventStoreClient = eventStoreClient;
      this.AggregateTypeName = typeof(TAggregate).Name.ToLower();
      this.AggregateType = typeof(TAggregate);
      this.EventTypeMap = eventTypeMap;
    }

    EventStoreClient EventStoreClient { get; }
    string AggregateTypeName { get; }
    Type AggregateType { get; }
    EventTypeRegistry EventTypeMap { get; }

    private string GetStreamName(Guid id) => $"{this.AggregateTypeName}-{id}";

    public async Task SaveAsync(TAggregate aggregate)
    {
      IPersistableAggregate persistable = aggregate;

      StreamRevision revision = persistable.OriginatingVersion == 0
        ? StreamRevision.None
        : StreamRevision.FromInt64(persistable.OriginatingVersion);

      var events = persistable.GetChanges();
      var eventData = new List<EventData>();
      PrepareEventData(events, eventData);

      IWriteResult result = await this.EventStoreClient.AppendToStreamAsync(
        this.GetStreamName(persistable.Id), revision, eventData);

      persistable.ChangesSaved(result.NextExpectedStreamRevision.ToInt64());
    }

    public async Task<TAggregate> LoadAsync(Guid id)
    {
      await using (var result = this.EventStoreClient.ReadStreamAsync(
        Direction.Forwards, this.GetStreamName(id), StreamPosition.Start))
      {
        if (await result.ReadState == ReadState.StreamNotFound)
        {
          return null;
        }
        var readEventTask = GetEvents(result);
        TAggregate aggregate = (TAggregate)Activator.CreateInstance(this.AggregateType, 
          BindingFlags.Instance | BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.NonPublic, 
          null, new object[] { id }, Thread.CurrentThread.CurrentCulture);
        IPersistableAggregate persistable = aggregate;

        (var domainEvents, var version) = await readEventTask;
        persistable.LoadFromEvents(domainEvents, version);
        
        return aggregate;
      }
    }

    async Task<(List<IDomainEvent>, long latestEventId)> GetEvents(EventStoreClient.ReadStreamResult streamResult)
    {
      var domainEvents = new List<IDomainEvent>();
      StreamPosition latestEventNumber = StreamPosition.Start;

      await foreach (var anEvent in streamResult)
      {
        Type domainEventType = this.EventTypeMap.GetType(anEvent.Event.EventType);
        IDomainEvent domainEvent = (IDomainEvent)JsonSerializer.Deserialize(anEvent.Event.Data.Span, domainEventType);

        domainEvents.Add(domainEvent);
        latestEventNumber = anEvent.Event.EventNumber;
      }
      return (domainEvents, latestEventNumber.ToInt64());
    }

    static void PrepareEventData(IEnumerable<IDomainEvent> events, List<EventData> eventData)
    {
      foreach (IDomainEvent domainEvent in events)
      {
        byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(domainEvent, domainEvent.GetType());

        EventData data = new EventData(Uuid.NewUuid(),
                                       domainEvent.GetType().Name.ToLower(),
                                       new ReadOnlyMemory<byte>(bytes));

        eventData.Add(data);
      }
    }
  }
}
