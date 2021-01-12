using Alex.DddBasics.Test.Domain;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Alex.DddBasics.Test.EventDispatcherTests
{
  [TestClass]
  public class Dispatching_Many_Events
  {
    [TestMethod]
    public void To_Many_Handler()
    {
      var citizen = Guid.NewGuid();
      var marriedToCitizen = Guid.NewGuid();
      var domainEvents = new List<IDomainEvent>()
      {
        new CitizenMovedEvent(citizen, "Springfield", "12345", "Evergreen Terrace", "5679", "USA"),
        new CitizenMarriedEvent(citizen, marriedToCitizen)
      };
      var sut = new EventDispatcher();

      sut.Dispatch(domainEvents);

      DomainEventHandlerStub.HandledEvents.Should().HaveCount(3);
      DomainEventHandlerStub.HandledEvents.First().Should().Match<CitizenMovedEvent>(domainEvent =>
        domainEvent.Citizen == citizen &&
        domainEvent.City == "Springfield" &&
        domainEvent.CityCode == "12345" &&
        domainEvent.Street == "Evergreen Terrace" &&
        domainEvent.HouseNumber == "5679", "USA");
      DomainEventHandlerStub.HandledEvents.Skip(1).First().Should().Match<CitizenMarriedEvent>(domainEvents =>
        domainEvents.Citizen == citizen &&
        domainEvents.MarriedToCitizen == marriedToCitizen);
      DomainEventHandlerStub.HandledEvents.Last().Should().Match<CitizenMarriedEvent>(domainEvents =>
        domainEvents.Citizen == citizen &&
        domainEvents.MarriedToCitizen == marriedToCitizen);
    }
  }
}
