using EventStore.Client;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Alex.DddBasics.EventStoreDB
{
  public class Repository<TAggregate> : IRepository<TAggregate> where TAggregate : AggregateRoot
  {
    public Repository(EventStoreClient eventStoreClient)
    {
      _ = eventStoreClient ?? throw new ArgumentNullException(nameof(eventStoreClient));

      this.EventStoreClient = eventStoreClient;
    }

    EventStoreClient EventStoreClient { get; }

    public async Task SaveAsync(TAggregate aggregate)
    {
      IPersistableAggregate persistable = aggregate;

      StreamRevision revision = persistable.OriginatingVersion == 0
        ? StreamRevision.None
        : StreamRevision.FromInt64(persistable.OriginatingVersion);

      var events = persistable.GetChanges();
      var eventData = new List<EventData>();

      foreach (IDomainEvent domainEvent in events)
      {
        byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(domainEvent, domainEvent.GetType());

        EventData data = new EventData(Uuid.NewUuid(), domainEvent.GetType().Name.ToLower(), new ReadOnlyMemory<byte>(bytes));

        eventData.Add(data);
      }

      IWriteResult result = await this.EventStoreClient.AppendToStreamAsync(
        $"{persistable.GetType().Name.ToLower()}-{persistable.Id}", revision, eventData);

      persistable.ChangesSaved(result.NextExpectedStreamRevision.ToInt64());
    }
  }
}
