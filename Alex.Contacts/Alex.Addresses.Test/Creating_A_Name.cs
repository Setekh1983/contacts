using CSharpFunctionalExtensions;

using FluentAssertions;

using System;

using Xunit;

namespace Alex.Addresses.Test
{
  public class Creating_A_Name
  {
    private const string MISSING_SURNAME_ERROR_MESSAGE = "Please provide a surname.";
    private const string MISSING_FORNAME_ERROR_MESSAGE = "Please provide a forename.";

    [Fact]
    public void With_Minimal_Parameters()
    {
      Result<Name> name = Name.Create("Homer", "Simpson");

      name.Should().NotBeNull();
      name.Value.Forename.Should().Be("Homer");
      name.Value.Surname.Should().Be("Simpson");
    }

    [Fact]
    public void Does_Not_Accept_An_Empty_Forename()
    {
      Result<Name> name = Name.Create("", "Simpson");

      name.IsFailure.Should().BeTrue();
      name.Error.Should().Be(MISSING_FORNAME_ERROR_MESSAGE);
    }

    [Fact]
    public void Does_Not_Accept_Null_As_A_Forename()
    {
      Result<Name> name = Name.Create(null, "Simpson");

      name.IsFailure.Should().BeTrue();
      name.Error.Should().Be(MISSING_FORNAME_ERROR_MESSAGE);
    }

    [Fact]
    public void Does_Not_Accept_Whitespace_As_A_Forename()
    {
      Result<Name> name = Name.Create("   ", "Simpson");

      name.IsFailure.Should().BeTrue();
      name.Error.Should().Be(MISSING_FORNAME_ERROR_MESSAGE);
    }

    [Fact]
    public void Does_Not_Accept_An_Empty_Surname()
    {
      Result<Name> name = Name.Create("Homer", "");

      name.IsFailure.Should().BeTrue();
      name.Error.Should().Be(MISSING_SURNAME_ERROR_MESSAGE);
    }

    [Fact]
    public void Does_Not_Accept_Null_As_A_Surname()
    {
      Result<Name> name = Name.Create("Homer", null);

      name.IsFailure.Should().BeTrue();
      name.Error.Should().Be(MISSING_SURNAME_ERROR_MESSAGE);
    }

    [Fact]
    public void Does_Not_Accept_Whitespace_As_A_Surname()
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
    }
  }
}
