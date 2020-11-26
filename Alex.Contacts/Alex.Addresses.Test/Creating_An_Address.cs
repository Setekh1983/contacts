using CSharpFunctionalExtensions;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alex.Addresses.Test
{
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
  [TestClass]
  public class Creating_An_Address
  {
    [TestMethod]
    public void With_All_Parameters()
    {
      Result<Address> sut = Address.Create("Springfield", "12345", "Evergreen Terrace", "742");

      sut.Should().NotBeNull();
      sut.Value.City.Should().Be("Springfield");
      sut.Value.CityCode.Should().Be("12345");
      sut.Value.Street.Should().Be("Evergreen Terrace");
      sut.Value.Housenumber.Should().Be("742");

      sut.Value.ToString().Should().Be(@"Evergreen Terrace 742
12345 Springfield");

      ((string)sut.Value).Should().Be(@"Evergreen Terrace 742
12345 Springfield");
    }

    [TestMethod]
    public void Requires_At_Least_One_Parameter()
    {
      Result<Address> sut = Address.Create(null, null, null, null);

      sut.IsFailure.Should().BeTrue();
      sut.Error.Should().Be("Please provide at least one value of the address.");
    }

    [TestMethod]
    public void Only_From_City()
    {
      Result<Address> sut = Address.Create("Springfield", null, null, null);

      sut.IsSuccess.Should().BeTrue();
      sut.Value.City.Should().Be("Springfield");
      sut.Value.CityCode.Should().BeNull();
      sut.Value.Street.Should().BeNull();
      sut.Value.Housenumber.Should().BeNull();
      sut.Value.ToString().Should().Be("Springfield");
      ((string)sut.Value).Should().Be("Springfield");
    }

    [TestMethod]
    public void Only_From_City_Code()
    {
      Result<Address> sut = Address.Create(null, "12345", null, null);

      sut.Should().NotBeNull();
      sut.Value.City.Should().BeNull();
      sut.Value.CityCode.Should().Be("12345");
      sut.Value.Street.Should().BeNull();
      sut.Value.Housenumber.Should().BeNull();
      sut.Value.ToString().Should().Be("12345");
      ((string)sut.Value).Should().Be("12345");
    }

    [TestMethod]
    public void Only_From_Street()
    {
      Result<Address> sut = Address.Create(null, null, "Evergreen Terrace", null);

      sut.Should().NotBeNull();
      sut.Value.City.Should().BeNull();
      sut.Value.CityCode.Should().BeNull();
      sut.Value.Street.Should().Be("Evergreen Terrace");
      sut.Value.Housenumber.Should().BeNull();
      sut.Value.ToString().Should().Be("Evergreen Terrace");
      ((string)sut.Value).Should().Be("Evergreen Terrace");
    }

    [TestMethod]
    public void Only_From_HouseNumber()
    {
      Result<Address> sut = Address.Create(null, null, null, "742");

      sut.Should().NotBeNull();
      sut.Value.City.Should().BeNull();
      sut.Value.CityCode.Should().BeNull();
      sut.Value.Street.Should().BeNull();
      sut.Value.Housenumber.Should().Be("742");
      sut.Value.ToString().Should().Be("742");
      ((string)sut.Value).Should().Be("742");
    }
  }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
}
