using Alex.DddBasics.Test.Domain;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;

namespace Alex.DddBasics.Test.AggregateRootTests
{
  [TestClass]
  public class Replaying_Events
  {
    [TestMethod]
    public void Recovers_The_State_Of_The_Aggregate()
    {
      var sut = new Citizen();
      var partner = new Citizen();
      var events = new List<IDomainEvent>();
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
    public void From_An_Empty_Collection_Raises_An_Error()
    {
      var sut = new Citizen();

      Action action = () => ((IPersistableAggregate)sut).LoadFromEvents(new List<IDomainEvent>(), 0);

      action.Should().Throw<ArgumentException>("Please provide a collection of events.");
    }

    [TestMethod]
    public void From_A_Null_Collection_Raises_An_Error()
    {
      var sut = new Citizen();

      Action action = () => ((IPersistableAggregate)sut).LoadFromEvents(null, 0);

      action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void With_A_Version_Smaller_Then_Zero_Raises_Error()
    {
      var sut = new Citizen();
      var events = new List<IDomainEvent>();
      events.Add(new CitizenMovedEvent(sut.Id, "", "", "", "", ""));

      Action action = () => ((IPersistableAggregate)sut).LoadFromEvents(events, -1);

      action.Should().Throw<ArgumentException>("Negative version numbers are not allowed. The version number must be at least zero.");
    }
  }
}
