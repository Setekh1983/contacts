using Alex.DddBasics.Test.Domain;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace Alex.DddBasics.Test.EntityTests
{
  [TestClass]
  public class Creating_An_Entity
  {
    [TestMethod]
    public void Generates_A_New_Id()
    {
      var sut = new Citizen();

      sut.Id.Should().NotBeEmpty();
    }

    [TestMethod]
    public void Uses_The_Provided_Id()
    {
      var id = Guid.NewGuid();

      var sut = new Citizen(id);

      sut.Id.Should().Be(id);
    }

    [TestMethod]
    public void Does_Not_Allow_Empty_Guids()
    {
      Action action = () => new Citizen(Guid.Empty);

      action.Should().Throw<ArgumentException>("An empty GUID is not allowed.");
    }
  }
}
