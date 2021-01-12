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
      var domainEvents = new List<IDomainEvent>()
      {
        new CitizenMovedEvent(citizen, "Springfield", "12345", "Evergreen Terrace", "5679", "USA")
      };
      var sut = new EventDispatcher();

      sut.Dispatch(domainEvents);

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
      var domainEvents = new List<IDomainEvent>
      {
        new CitizenMarriedEvent(citizen, marriedToCitizen)
      };
      var sut = new EventDispatcher();

      sut.Dispatch(domainEvents);

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
      var domainEvents = new List<IDomainEvent>
      {
        new CitizenDiedEvent(citizen)
      };
      var sut = new EventDispatcher();

      sut.Dispatch(domainEvents);

      DomainEventHandlerStub.HandledEvents.Should().BeEmpty();
    }
  }
}
