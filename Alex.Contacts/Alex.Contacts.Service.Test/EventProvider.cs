using Alex.DddBasics;
using Alex.DddBasics.EventStoreDB;

using EventStore.Client;

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Alex.Contacts.Service.Test
{
  public static class EventProvider
  {
    const string CONNECTION_STRING = "esdb://admin:changit@localhost:2113?Tls=false";

    static EventTypeRegistry? EventTypeRegistry;

    public static EventStoreClient GetEventStoreClient()
    {
      var settings = EventStoreClientSettings.Create(CONNECTION_STRING);

      return new EventStoreClient(settings);
    }

    public static EventTypeRegistry GetEventTypeMap()
    {
      if (EventTypeRegistry is null)
      {
        EventTypeRegistry = EventTypeRegistry.LoadFromAssembly(typeof(ContactCreated).Assembly);
      }
      return EventTypeRegistry;
    }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.

    public async static Task<List<IDomainEvent>> GetEvents<TStream>(Guid aggregateId)
    {
      var streamName = $"{typeof(TStream).Name.ToLower()}-{aggregateId}";
      var domainEvents = new List<IDomainEvent>();

      var client = GetEventStoreClient();
      var eventTypeMap = GetEventTypeMap();

      EventStoreClient.ReadStreamResult result = client.ReadStreamAsync(Direction.Forwards, streamName, StreamPosition.Start);

      await foreach (var anEvent in result)
      {
        Type domainEventType = eventTypeMap.GetType(anEvent.Event.EventType);
        var domainEvent = (IDomainEvent)JsonSerializer.Deserialize(anEvent.Event.Data.Span, domainEventType);

        domainEvents.Add(domainEvent);
      }
      return domainEvents;
    }

#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

    public static IRepository<TAggregate> GetRepository<TAggregate>() where TAggregate : AggregateRoot =>
      new Repository<TAggregate>(GetEventStoreClient(), GetEventTypeMap());
  }
}
