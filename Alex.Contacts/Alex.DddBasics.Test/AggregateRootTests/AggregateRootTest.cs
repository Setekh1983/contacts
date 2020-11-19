using Alex.DddBasics.Test.Domain;

using FluentAssertions;

using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace Alex.DddBasics.Test.AggregateRootTests
{
  public class AggregateRootTest
  {
    [Fact]
    public void A_New_Aggregate_Has_No_Changes()
    {
      Citizen sut = new Citizen();
      IEnumerable<IDomainEvent> events = sut.GetChanges();

      events.Should().BeEmpty();
    }

    [Fact]
    public void Executing_An_Action_Adds_An_Event_To_The_Changes()
    {
      Citizen homer = new Citizen();
      Citizen marge = new Citizen();

      homer.Marry(marge);
      IEnumerable<IDomainEvent> events = homer.GetChanges();

      events.Should().HaveCount(1);
      events.First().Should().Match<CitizenMarriedEvent>(domainEvent =>
        domainEvent.Citizen == homer.Id &&
        domainEvent.MarriedToCitizen == marge.Id);
    }

    [Fact]
    public void Executing_Multiple_Actions_Events_Are_Added_In_Order()
    {
      Citizen homer = new Citizen();
      Citizen marge = new Citizen();
      Address address = new Address("Springfield", "12345", "Evergreen Terrace", "123", "USA");

      homer.Marry(marge);
      homer.Move(address);

      IEnumerable<IDomainEvent> events = homer.GetChanges();

      events.Should().HaveCount(2);
      events.First().Should().Match<CitizenMarriedEvent>(domainEvent =>
        domainEvent.Citizen == homer.Id &&
        domainEvent.MarriedToCitizen == marge.Id);

      events.Last().Should().Match<CitizenMovedEvent>(domainEvent =>
        domainEvent.Citizen == homer.Id &&
        domainEvent.City == address.City &&
        domainEvent.CityCode == address.CityCode &&
        domainEvent.Street == address.Street &&
        domainEvent.HouseNumber == address.HouseNumber &&
        domainEvent.Country == address.Country);
    }

    [Fact]
    public void All_Events_Can_Be_Removed()
    {
      Citizen homer = new Citizen();
      Citizen marge = new Citizen();
      Address address = new Address("Springfield", "12345", "Evergreen Terrace", "123", "USA");

      homer.Marry(marge);
      homer.Move(address);

      homer.ClearEvents();
      IEnumerable<IDomainEvent> events = homer.GetChanges();

      events.Should().BeEmpty();
    }
  }
}
