
using Alex.DddBasics.Test.Domain;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;

namespace Alex.DddBasics.Test.EventDispatcherTests
{
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8631 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match constraint type.
  [TestClass]
  public class Dispatching_Null
  {
    [TestMethod]
    public void As_Single_Event_Raises_An_Error()
    {
      IDomainEvent domainEvent = null;
      var handlerResults = new DomainEventHandlerStub();
      IDomainEventDispatcher sut = new DomainEventDispatcher(handlerResults.CreateHandler);

      Action action = () => sut.Dispatch(domainEvent).GetAwaiter().GetResult();

      action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void As_Collection_Raises_An_Error()
    {
      IList<IDomainEvent>? domainEvent = null;
      var handlerResults = new DomainEventHandlerStub();
      IDomainEventDispatcher sut = new DomainEventDispatcher(handlerResults.CreateHandler);

      Action action = () => sut.Dispatch(domainEvent).GetAwaiter().GetResult();

      action.Should().Throw<ArgumentNullException>();
    }
  }
#pragma warning restore CS8631 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match constraint type.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
}
