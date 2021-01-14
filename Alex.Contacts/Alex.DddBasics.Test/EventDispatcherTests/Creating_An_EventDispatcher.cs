using Alex.DddBasics.Test.Domain;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Alex.DddBasics.Test.EventDispatcherTests
{
  [TestClass]
  public class Creating_An_EventDispatcher
  {
    [TestMethod]
    public void Requires_A_FactoryFunction()
    {
      IDomainEventDispatcher sut = new DomainEventDispatcher(type => new object());

      sut.Should().NotBeNull();
    }

    [TestMethod]
    public void With_Null_As_A_FactoryFunction_Raises_An_Error()
    {
      Action action = () => new DomainEventDispatcher(null);

      action.Should().Throw<ArgumentNullException>();
    }
  }
}
