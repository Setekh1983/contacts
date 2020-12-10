using Alex.DddBasics.EventStoreDB.Test.Domain;

using EventStore.Client;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alex.DddBasics.EventStoreDB.Test
{
  [TestClass]
  public class Saving_An_Aggregate
  {
    const string CONNECTION_STRING = "esdb://admin:changit@localhost:2113?Tls=false";

    protected static EventStoreClient GetEventStoreClient()
    {
      var settings = EventStoreClientSettings.Create(CONNECTION_STRING);

      return new EventStoreClient(settings);
    }

    [TestMethod]
    public void Stores_All_Events_In_Order()
    {
      var homer = new Citizen();
      var marge = new Citizen();

      homer.Marry(marge);
      var address = new Address("Springfield", "12345", "Evergreen Terrace", "7890", "USA");
      homer.Move(address);

      var repo = new Repository<Citizen>(GetEventStoreClient());
      repo.SaveAsync(homer).GetAwaiter().GetResult();

      var streamName = $"{homer.GetType().Name.ToLower()}-{homer.Id}";

      EventStoreClient client = GetEventStoreClient();
      EventStoreClient.ReadStreamResult streamResult = client.ReadStreamAsync(
        Direction.Forwards, streamName, StreamPosition.Start);

      streamResult.ReadState.GetAwaiter().GetResult().Should().Be(ReadState.Ok);

      var events = streamResult.ToListAsync().GetAwaiter().GetResult();

      string firstEventJson = Encoding.UTF8.GetString(events.First().Event.Data.ToArray());
      CitizenMarriedEvent? firstEvent = System.Text.Json.JsonSerializer.Deserialize<CitizenMarriedEvent>(firstEventJson);

      firstEvent.Should().Match<CitizenMarriedEvent>(domainEvent =>
        domainEvent.Citizen == homer.Id &&
        domainEvent.MarriedToCitizen == marge.Id);

      string lastEventJson = Encoding.UTF8.GetString(events.Last().Event.Data.ToArray());
      CitizenMovedEvent? lastEvent = System.Text.Json.JsonSerializer.Deserialize<CitizenMovedEvent>(lastEventJson);

      lastEvent.Should().Match<CitizenMovedEvent>(domainEvent =>
        domainEvent.City == address.City &&
        domainEvent.CityCode == address.CityCode &&
        domainEvent.Street == address.Street &&
        domainEvent.HouseNumber == address.HouseNumber);
    }

    [TestMethod]
    public void Clears_All_Events_From_The_Aggregate_And_Sets_The_New_Version()
    {
      var homer = new Citizen();
      var marge = new Citizen();

      homer.Marry(marge);
      var address = new Address("Springfield", "12345", "Evergreen Terrace", "7890", "USA");
      homer.Move(address);

      var repo = new Repository<Citizen>(GetEventStoreClient());
      repo.SaveAsync(homer).GetAwaiter().GetResult();

      IEnumerable<IDomainEvent> events = homer.GetChanges();

      events.Should().BeEmpty();
      ((IPersistableAggregate)homer).OriginatingVersion.Should().Be(1);
    }
  }
}
