using Alex.DddBasics.Test.Domain;

using FluentAssertions;

using System;

using Xunit;

namespace Alex.DddBasics.Test.EntityTests
{
  public class Creating_An_Entity
  {
    [Fact]
    public void Generates_A_New_Id()
    {
      var sut = new Citizen();

      sut.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Uses_The_Provided_Id()
    {
      var id = Guid.NewGuid();

      var sut = new Citizen(id);

      sut.Id.Should().Be(id);
    }

    [Fact]
    public void Does_Not_Allow_Empty_Guids()
    {
      Action action = () => new Citizen(Guid.Empty);

      action.Should().Throw<ArgumentException>("An empty GUID is not allowed.");
    }
  }
}
