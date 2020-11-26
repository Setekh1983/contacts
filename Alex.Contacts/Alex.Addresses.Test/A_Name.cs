using CSharpFunctionalExtensions;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alex.Addresses.Test
{
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8604 // Possible null reference argument.
  [TestClass]
  public class A_Name
  {
    private const string MISSING_SURNAME_ERROR_MESSAGE = "Please provide a surname.";
    private const string MISSING_FORNAME_ERROR_MESSAGE = "Please provide a forename.";

    [TestMethod]
    public void Can_Be_Created_With_Forename_And_Surname()
    {
      Result<Name> sut = Name.Create("Homer", "Simpson");

      sut.Should().NotBeNull();
      sut.Value.FirstName.Should().Be("Homer");
      sut.Value.LastName.Should().Be("Simpson");
      sut.Value.ToString().Should().Be("Homer Simpson");
      ((string)sut.Value).Should().Be("Homer Simpson");
    }

    [TestMethod]
    public void Cannot_Be_Created_With_An_Empty_Forename()
    {
      Result<Name> sut = Name.Create("", "Simpson");

      sut.IsFailure.Should().BeTrue();
      sut.Error.Should().Be(MISSING_FORNAME_ERROR_MESSAGE);
    }

    [TestMethod]
    public void Cannot_Be_Created_With_Null_As_A_Forename()
    {
      Result<Name> sut = Name.Create(null, "Simpson");

      sut.IsFailure.Should().BeTrue();
      sut.Error.Should().Be(MISSING_FORNAME_ERROR_MESSAGE);
    }

    [TestMethod]
    public void Cannot_Be_Created_With_Whitespace_As_A_Forename()
    {
      Result<Name> sut = Name.Create("   ", "Simpson");

      sut.IsFailure.Should().BeTrue();
      sut.Error.Should().Be(MISSING_FORNAME_ERROR_MESSAGE);
    }

    [TestMethod]
    public void Cannot_Be_Created_With_An_Empty_Surname()
    {
      Result<Name> sut = Name.Create("Homer", "");

      sut.IsFailure.Should().BeTrue();
      sut.Error.Should().Be(MISSING_SURNAME_ERROR_MESSAGE);
    }

    [TestMethod]
    public void Cannot_Be_Created_With_Null_As_A_Surname()
    {
      Result<Name> sut = Name.Create("Homer", null);

      sut.IsFailure.Should().BeTrue();
      sut.Error.Should().Be(MISSING_SURNAME_ERROR_MESSAGE);
    }

    [TestMethod]
    public void Cannot_Be_Created_With_Whitespace_As_A_Surname()
    {
      Result<Name> sut = Name.Create("Homer", "   ");

      sut.IsFailure.Should().BeTrue();
      sut.Error.Should().Be(MISSING_SURNAME_ERROR_MESSAGE);
    }

    [TestMethod]
    public void Removes_Trailing_And_Leading_Spaces_From_Forename_And_Surname()
    {
      Result<Name> sut = Name.Create("    Homer   ", "   Simpson    ");

      sut.IsSuccess.Should().BeTrue();
      sut.Value.FirstName.Should().Be("Homer");
      sut.Value.LastName.Should().Be("Simpson");
      sut.Value.ToString().Should().Be("Homer Simpson");
      ((string)sut.Value).Should().Be("Homer Simpson");
    }

    [TestMethod]
    public void Containing_Null_Can_Be_Cast_To_An_Empty_String()
    {
      Name? sut = null;

      ((string)sut).Should().BeEmpty();
    }
  }
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
}
