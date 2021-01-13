
using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;

namespace Alex.DddBasics.Test.EventDispatcherTests
{
  [TestClass]
  public class Dispatching_Null
  {
    [TestMethod]
    public void As_Single_Event_Raises_An_Error()
    {
      IDomainEvent? domainEvent = null;
      IDomainEventDispatcher sut = new DomainEventDispatcher();

      Action action = () => sut.Dispatch(domainEvent).GetAwaiter().GetResult();

      action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void As_Collection_Raises_An_Error()
    {
      IList<IDomainEvent>? domainEvent = null;
      IDomainEventDispatcher sut = new DomainEventDispatcher();

      Action action = () => sut.Dispatch(domainEvent).GetAwaiter().GetResult();

      action.Should().Throw<ArgumentNullException>();
    }
  }
}
