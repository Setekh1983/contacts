using Alex.DddBasics.Test.Domain;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;

namespace Alex.DddBasics.Test.EventDispatcherTests
{
  [TestClass]
  public class Dispatching_No_Events
  {
    [TestMethod]
    public void Does_Not_Throw_Or_Call_Handlers()
    {
      var domainEvents = new List<IDomainEvent>();
      var sut = new EventDispatcher();

      sut.Dispatch(domainEvents);

      DomainEventHandlerStub.HandledEvents.Should().BeEmpty();
    }
  }
}
