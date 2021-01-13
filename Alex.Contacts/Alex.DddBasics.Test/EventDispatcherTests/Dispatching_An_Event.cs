using Alex.DddBasics.Test.Domain;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Alex.DddBasics.Test.EventDispatcherTests
{
  [TestClass]
  public class Dispatching_An_Event
  {
    [TestMethod]
    public void To_One_Handler()
    {
      var citizen = Guid.NewGuid();
      var marriedToCitizen = Guid.NewGuid();
      var domainEvent = new CitizenMovedEvent(citizen, "Springfield", "12345", "Evergreen Terrace", "5679", "USA");
      IDomainEventDispatcher sut = new DomainEventDispatcher();

      sut.Dispatch(domainEvent).GetAwaiter().GetResult();

      DomainEventHandlerStub.HandledEvents.Should().HaveCount(1);
      DomainEventHandlerStub.HandledEvents.First().Should().Match<CitizenMovedEvent>(domainEvent =>
        domainEvent.Citizen == citizen &&
        domainEvent.City == "Springfield" &&
        domainEvent.CityCode == "12345" &&
        domainEvent.Street == "Evergreen Terrace" &&
        domainEvent.HouseNumber == "5679", "USA");
    }

    [TestMethod]
    public void To_Many_Handlers()
    {
      var citizen = Guid.NewGuid();
      var marriedToCitizen = Guid.NewGuid();
      var domainEvent = new CitizenMarriedEvent(citizen, marriedToCitizen);
      IDomainEventDispatcher sut = new DomainEventDispatcher();

      sut.Dispatch(domainEvent).GetAwaiter().GetResult();

      DomainEventHandlerStub.HandledEvents.Should().HaveCount(2);
      DomainEventHandlerStub.HandledEvents.First().Should().Match<CitizenMarriedEvent>(domainEvent =>
        domainEvent.Citizen == citizen &&
        domainEvent.MarriedToCitizen == marriedToCitizen);
      DomainEventHandlerStub.HandledEvents.Last().Should().Match<CitizenMarriedEvent>(domainEvent =>
        domainEvent.Citizen == citizen &&
        domainEvent.MarriedToCitizen == marriedToCitizen);
    }
    [TestMethod]
    public void To_No_Handler()
    {
      var citizen = Guid.NewGuid();
      var domainEvent = new CitizenDiedEvent(citizen);
      IDomainEventDispatcher sut = new DomainEventDispatcher();

      sut.Dispatch(domainEvent).GetAwaiter().GetResult();

      DomainEventHandlerStub.HandledEvents.Should().BeEmpty();
    }
  }
}
