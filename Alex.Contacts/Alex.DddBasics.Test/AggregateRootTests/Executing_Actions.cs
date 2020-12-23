using Alex.DddBasics.Test.Domain;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Linq;

namespace Alex.DddBasics.Test.AggregateRootTests
{
  [TestClass]
  public class Executing_Actions
  {
    [TestMethod]
    public void Adds_Events_To_The_Changes()
    {
      var homer = new Citizen();
      var marge = new Citizen();

      homer.Marry(marge);
      IEnumerable<IDomainEvent> events = homer.GetChanges();

      events.Should().HaveCount(1);
      events.First().Should().Match<CitizenMarriedEvent>(domainEvent =>
        domainEvent.Citizen == homer.Id &&
        domainEvent.MarriedToCitizen == marge.Id);
    }

    [TestMethod]
    public void Adds_Events_In_The_Order_The_Action_Were_Executed()
    {
      var homer = new Citizen();
      var marge = new Citizen();
      var address = new Address("Springfield", "12345", "Evergreen Terrace", "123", "USA");

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
  }
}
