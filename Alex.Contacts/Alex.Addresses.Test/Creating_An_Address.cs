using CSharpFunctionalExtensions;

using FluentAssertions;

using System;

using Xunit;

namespace Alex.Addresses.Test
{
  public class Creating_An_Address
  {
    [Fact]
    public void With_All_Parameters()
    {
      Result<Address> address = Address.Create("Springfield", "12345", "Evergreen Terrace", "742");

      address.Should().NotBeNull();
      address.Value.City.Should().Be("Springfield");
      address.Value.CityCode.Should().Be("12345");
      address.Value.Street.Should().Be("Evergreen Terrace");
      address.Value.Housenumber.Should().Be("742");

      address.Value.ToString().Should().Be(@"Evergreen Terrace 742
12345 Springfield");
    }

    [Fact]
    public void Requires_At_Least_One_Parameter()
    {
      Result<Address> address = Address.Create(null, null, null, null);

      address.IsFailure.Should().BeTrue();
      address.Error.Should().Be("Please provide at least one value of the address.");
    }

    [Fact]
    public void Only_From_City()
    {
      Result<Address> address = Address.Create("Springfield", null, null, null);

      address.IsSuccess.Should().BeTrue();
      address.Value.City.Should().Be("Springfield");
      address.Value.CityCode.Should().BeNull();
      address.Value.Street.Should().BeNull();
      address.Value.Housenumber.Should().BeNull();
      address.Value.ToString().Should().Be("Springfield");
    }

    [Fact]
    public void Only_From_City_Code()
    {
      Result<Address> address = Address.Create(null, "12345", null, null);

      address.Should().NotBeNull();
      address.Value.City.Should().BeNull();
      address.Value.CityCode.Should().Be("12345");
      address.Value.Street.Should().BeNull();
      address.Value.Housenumber.Should().BeNull();
      address.Value.ToString().Should().Be("12345");
    }

    [Fact]
    public void Only_From_Street()
    {
      Result<Address> address = Address.Create(null, null, "Evergreen Terrace", null);

      address.Should().NotBeNull();
      address.Value.City.Should().BeNull();
      address.Value.CityCode.Should().BeNull();
      address.Value.Street.Should().Be("Evergreen Terrace");
      address.Value.Housenumber.Should().BeNull();
      address.Value.ToString().Should().Be("Evergreen Terrace");
    }

    [Fact]
    public void Only_From_HouseNumber()
    {
      Result<Address> address = Address.Create(null, null, null, "742");

      address.Should().NotBeNull();
      address.Value.City.Should().BeNull();
      address.Value.CityCode.Should().BeNull();
      address.Value.Street.Should().BeNull();
      address.Value.Housenumber.Should().Be("742");
      address.Value.ToString().Should().Be("742");
    }
  }
}
