using CSharpFunctionalExtensions;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alex.Addresses.Test
{
  [TestClass]
  public class A_New_Name
  {
    private const string MISSING_VALUE_ERROR_MESSAGE = "Please provide a value.";
    
    [TestMethod]
    public void Requires_A_Name()
    {
      Result<Name> sut = Name.Create("Homer");

      sut.Should().NotBeNull();
      sut.Value.Value.Should().Be("Homer");
      sut.Value.ToString().Should().Be("Homer");
      ((string)sut.Value).Should().Be("Homer");
    }

    [TestMethod]
    public void Cannot_Be_Created_With_An_Empty_Value()
    {
      Result<Name> sut = Name.Create("");

      sut.IsFailure.Should().BeTrue();
      sut.Error.Should().Be(MISSING_VALUE_ERROR_MESSAGE);
    }

    [TestMethod]
    public void Cannot_Be_Created_With_Null_As_A_Value()
    {
      Result<Name> sut = Name.Create(null);

      sut.IsFailure.Should().BeTrue();
      sut.Error.Should().Be(MISSING_VALUE_ERROR_MESSAGE);
    }

    [TestMethod]
    public void Cannot_Be_Created_With_Whitespace_As_A_Value()
    {
      Result<Name> sut = Name.Create("   ");

      sut.IsFailure.Should().BeTrue();
      sut.Error.Should().Be(MISSING_VALUE_ERROR_MESSAGE);
    }

    [TestMethod]
    public void Removes_Trailing_And_Leading_Spaces_From_Value()
    {
      Result<Name> sut = Name.Create("    Homer   ");

      sut.IsSuccess.Should().BeTrue();
      sut.Value.Value.Should().Be("Homer");
      sut.Value.ToString().Should().Be("Homer");
      ((string)sut.Value).Should().Be("Homer");
    }
  }
}
