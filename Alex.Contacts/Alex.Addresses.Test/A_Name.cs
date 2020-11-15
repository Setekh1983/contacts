using CSharpFunctionalExtensions;

using FluentAssertions;

using Xunit;

namespace Alex.Addresses.Test
{
  public class A_Name
  {
    private const string MISSING_SURNAME_ERROR_MESSAGE = "Please provide a surname.";
    private const string MISSING_FORNAME_ERROR_MESSAGE = "Please provide a forename.";

    [Fact]
    public void Can_Be_Created_With_Forename_And_Surname()
    {
      Result<Name> name = Name.Create("Homer", "Simpson");

      name.Should().NotBeNull();
      name.Value.Forename.Should().Be("Homer");
      name.Value.Surname.Should().Be("Simpson");
      name.Value.ToString().Should().Be("Homer Simpson");
      ((string)name.Value).Should().Be("Homer Simpson");
    }

    [Fact]
    public void Cannot_Be_Created_With_An_Empty_Forename()
    {
      Result<Name> name = Name.Create("", "Simpson");

      name.IsFailure.Should().BeTrue();
      name.Error.Should().Be(MISSING_FORNAME_ERROR_MESSAGE);
    }

    [Fact]
    public void Cannot_Be_Created_With_Null_As_A_Forename()
    {
      Result<Name> name = Name.Create(null, "Simpson");

      name.IsFailure.Should().BeTrue();
      name.Error.Should().Be(MISSING_FORNAME_ERROR_MESSAGE);
    }

    [Fact]
    public void Cannot_Be_Created_With_Whitespace_As_A_Forename()
    {
      Result<Name> name = Name.Create("   ", "Simpson");

      name.IsFailure.Should().BeTrue();
      name.Error.Should().Be(MISSING_FORNAME_ERROR_MESSAGE);
    }

    [Fact]
    public void Cannot_Be_Created_With_An_Empty_Surname()
    {
      Result<Name> name = Name.Create("Homer", "");

      name.IsFailure.Should().BeTrue();
      name.Error.Should().Be(MISSING_SURNAME_ERROR_MESSAGE);
    }

    [Fact]
    public void Cannot_Be_Created_With_Null_As_A_Surname()
    {
      Result<Name> name = Name.Create("Homer", null);

      name.IsFailure.Should().BeTrue();
      name.Error.Should().Be(MISSING_SURNAME_ERROR_MESSAGE);
    }

    [Fact]
    public void Cannot_Be_Created_With_Whitespace_As_A_Surname()
    {
      Result<Name> name = Name.Create("Homer", "   ");

      name.IsFailure.Should().BeTrue();
      name.Error.Should().Be(MISSING_SURNAME_ERROR_MESSAGE);
    }

    [Fact]
    public void Removes_Trailing_And_Leading_Spaces_From_Forename_And_Surname()
    {
      Result<Name> name = Name.Create("    Homer   ", "   Simpson    ");

      name.IsSuccess.Should().BeTrue();
      name.Value.Forename.Should().Be("Homer");
      name.Value.Surname.Should().Be("Simpson");
      name.Value.ToString().Should().Be("Homer Simpson");
      ((string)name.Value).Should().Be("Homer Simpson");
    }

    [Fact]
    public void Containing_Null_Can_Be_Cast_To_An_Empty_String()
    {
      Name? name = null;

      ((string)name).Should().BeEmpty();
    }
  }
}
