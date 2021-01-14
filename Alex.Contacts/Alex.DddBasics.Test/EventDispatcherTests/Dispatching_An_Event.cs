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
      var handlerResults = new DomainEventHandlerStub();
      IDomainEventDispatcher sut = new DomainEventDispatcher(handlerResults.CreateHandler);

      sut.Dispatch(domainEvent).GetAwaiter().GetResult();

      handlerResults.HandledEvents.Should().HaveCount(1);
      handlerResults.HandledEvents.First().Should().Match<CitizenMovedEvent>(domainEvent =>
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
      var handlerResults = new DomainEventHandlerStub();
      IDomainEventDispatcher sut = new DomainEventDispatcher(handlerResults.CreateHandler);

      sut.Dispatch(domainEvent).GetAwaiter().GetResult();

      handlerResults.HandledEvents.Should().HaveCount(2);
      handlerResults.HandledEvents.First().Should().Match<CitizenMarriedEvent>(domainEvent =>
        domainEvent.Citizen == citizen &&
        domainEvent.MarriedToCitizen == marriedToCitizen);
      handlerResults.HandledEvents.Last().Should().Match<CitizenMarriedEvent>(domainEvent =>
        domainEvent.Citizen == citizen &&
        domainEvent.MarriedToCitizen == marriedToCitizen);
    }
    [TestMethod]
    public void To_No_Handler()
    {
      var citizen = Guid.NewGuid();
      var domainEvent = new CitizenDiedEvent(citizen);
      var handlerResults = new DomainEventHandlerStub();
      IDomainEventDispatcher sut = new DomainEventDispatcher(handlerResults.CreateHandler);

      sut.Dispatch(domainEvent).GetAwaiter().GetResult();

      handlerResults.HandledEvents.Should().BeEmpty();
    }


  }
}
