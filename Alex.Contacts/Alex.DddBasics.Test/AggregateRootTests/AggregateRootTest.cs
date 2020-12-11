using Alex.DddBasics.Test.Domain;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Alex.DddBasics.Test.AggregateRootTests
{
  [TestClass]
  public class AggregateRootTest
  {
    [TestMethod]
    public void A_New_Aggregate_Has_No_Changes()
    {
      var sut = new Citizen();
      IEnumerable<IDomainEvent> events = sut.GetChanges();

      events.Should().BeEmpty();
    }

    [TestMethod]
    public void The_Originating_Version_Of_New_Aggregate_Is_Zero()
    {
      IPersistableAggregate sut = new Citizen();

      sut.OriginatingVersion.Should().Be(0);
    }


    [TestMethod]
    public void Executing_An_Action_Adds_An_Event_To_The_Changes()
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
    public void Executing_Multiple_Actions_Adds_Events_In_Order()
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

    [TestMethod]
    public void Saving_Changes_Clears_The_Changes_And_Sets_The_New_Version()
    {
      var homer = new Citizen();
      var marge = new Citizen();
      var address = new Address("Springfield", "12345", "Evergreen Terrace", "123", "USA");

      homer.Marry(marge);
      homer.Move(address);

      IPersistableAggregate aggregate = homer;
      aggregate.ChangesSaved(2);

      aggregate.GetChanges().Should().BeEmpty();
      aggregate.OriginatingVersion.Should().Be(2);
    }

    [TestMethod]
    public void Setting_A_Lower_Version_Then_The_Current_Causes_Error()
    {
      var sut = new Citizen();
      var partner = new Citizen();

      List<IDomainEvent> events = new List<IDomainEvent>();
      events.Add(new CitizenMovedEvent(sut.Id, "Springfield", "12345", "Evergreeen Terrace", "1234", "USA"));
      events.Add(new CitizenMarriedEvent(sut.Id, partner.Id));

      ((IPersistableAggregate)sut).LoadFromEvents(events, 1);

      Action action = () => ((IPersistableAggregate)sut).ChangesSaved(0);

      action.Should().Throw<ArgumentException>("The new version must be greater than the previous version.");
    }

    [TestMethod]
    public void Recovers_State_From_Events()
    {
      Citizen sut = new Citizen();
      Citizen partner = new Citizen();
      List<IDomainEvent> events = new List<IDomainEvent>();
      events.Add(new CitizenMovedEvent(sut.Id, "Springfield", "12345", "Evergreeen Terrace", "1234", "USA"));
      events.Add(new CitizenMarriedEvent(sut.Id, partner.Id));

      IPersistableAggregate aggregate = sut;
      aggregate.LoadFromEvents(events, 1);

      aggregate.GetChanges().Should().HaveCount(0);
      aggregate.OriginatingVersion.Should().Be(1);
      sut.Partner.Should().Be(partner.Id);
      sut.Address.Should().Be(new Address("Springfield", "12345", "Evergreeen Terrace", "1234", "USA"));
    }

    [TestMethod]
    public void Recovering_From_Empty_Events_Raises_Error()
    {
      var sut = new Citizen();

      Action action = () => ((IPersistableAggregate)sut).LoadFromEvents(new List<IDomainEvent>(), 0);

      action.Should().Throw<ArgumentException>("Please provide a collection of events.");
    }

    [TestMethod]
    public void Recovering_From_A_Null_Collection_Raises_Error()
    {
      var sut = new Citizen();

      Action action = () => ((IPersistableAggregate)sut).LoadFromEvents(null, 0);

      action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void Recovering_With_A_Version_Smaller_Then_Zero_Raises_Error()
    {
      var sut = new Citizen();
      List<IDomainEvent> events = new List<IDomainEvent>();
      events.Add(new CitizenMovedEvent(sut.Id, "", "", "", "", ""));

      Action action = () => ((IPersistableAggregate)sut).LoadFromEvents(events, -1);

      action.Should().Throw<ArgumentException>("Negative version numbers are not allowed. The version number must be at least zero.");
    }
  }
}
