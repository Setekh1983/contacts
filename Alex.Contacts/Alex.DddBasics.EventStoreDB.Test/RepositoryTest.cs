using EventStore.Client;

namespace Alex.DddBasics.EventStoreDB.Test
{
  public class RepositoryTest
  {
    const string CONNECTION_STRING = "esdb://admin:changit@localhost:2113?Tls=false";

    static EventTypeRegistry EventTypeRegistry = null;

    protected static EventStoreClient GetEventStoreClient()
    {
      var settings = EventStoreClientSettings.Create(CONNECTION_STRING);

      return new EventStoreClient(settings);
    }

    protected static EventTypeRegistry GetEventTypeMap()
    {
      if (EventTypeRegistry is null)
      {
        EventTypeRegistry = EventTypeRegistry.LoadFromAssembly(typeof(RepositoryTest).Assembly);
      }
      return EventTypeRegistry;
    }
  }
}
