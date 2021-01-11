using CSharpFunctionalExtensions;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alex.Contacts.NameTest
{
  [TestClass]
  public class A_New_Name
  {
    private const string MISSING_SURNAME_ERROR_MESSAGE = "Please provide a surname.";
    private const string MISSING_FORNAME_ERROR_MESSAGE = "Please provide a forename.";

    [TestMethod]
    public void Requires_A_Forename_And_Surname()
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
  }
}
