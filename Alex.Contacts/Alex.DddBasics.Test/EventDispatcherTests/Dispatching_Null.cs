using Alex.DddBasics.Test.Domain;

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
    public void Raises_An_Error()
    {
      var sut = new EventDispatcher();

      Action action = () => sut.Dispatch(null);

      action.Should().Throw<ArgumentNullException>();
    }
  }
}
