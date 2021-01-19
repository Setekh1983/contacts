using Alex.DddBasics.EventStoreDB.Test.Domain;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace Alex.DddBasics.EventStoreDB.Test
{
  [TestClass]
  public class A_New_Repository : RepositoryTest
  {
    [TestMethod]
    public void Requires_An_EventStoreClient()
    {
      var handlerResult = new DomainEventHandlerStub();
      IDomainEventDispatcher domainEventDispatcher = new DomainEventDispatcher(handlerResult.CreateHandler);

      Action action = () => new Repository<Citizen>(null, GetEventTypeMap(), domainEventDispatcher);

      action.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'eventStoreClient')");
    }

    [TestMethod]
    public void Requires_An_EventTypeMap()
    {
      var handlerResult = new DomainEventHandlerStub();
      IDomainEventDispatcher domainEventDispatcher = new DomainEventDispatcher(handlerResult.CreateHandler);

      Action action = () => new Repository<Citizen>(GetEventStoreClient(), null, domainEventDispatcher);

      action.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'eventTypeMap')");
    }

    [TestMethod]
    public void Requires_A_DomainEventDispatcher()
    {
      var handlerResult = new DomainEventHandlerStub();
      IDomainEventDispatcher domainEventDispatcher = new DomainEventDispatcher(handlerResult.CreateHandler);

      Action action = () => new Repository<Citizen>(GetEventStoreClient(), GetEventTypeMap(), null);

      action.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'domainEventDispatcher')");
    }

    [TestMethod]
    public void Is_Created_Properly()
    {
      var handlerResult = new DomainEventHandlerStub();
      IDomainEventDispatcher domainEventDispatcher = new DomainEventDispatcher(handlerResult.CreateHandler);

      var sut = new Repository<Citizen>(GetEventStoreClient(), GetEventTypeMap(), domainEventDispatcher);

      sut.Should().NotBeNull();
    }
  }
}
