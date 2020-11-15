using CSharpFunctionalExtensions;

using System;
using System.Collections.Generic;

namespace Alex.Addresses
{
  public class Address : ValueObject
  {
    private Address(string city, string cityCode, string street, string houseNumber)
    {
      this.City = city;
      this.CityCode = cityCode;
      this.Street = street;
      this.Housenumber = houseNumber;
    }

    public string Housenumber { get; }
    public string Street { get; }
    public string CityCode { get; }
    public string City { get; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
      yield return this.CityCode;
      yield return this.City;
      yield return this.Street;
      yield return this.Housenumber;
    }

    public override string ToString()
    {
      string address = this.Street;

      if (!string.IsNullOrWhiteSpace(this.Housenumber))
      {
        if (!string.IsNullOrWhiteSpace(this.Street))
        {
          address = string.Concat(address, " ", this.Housenumber);
        }
        else
        {
          address = string.Concat(this.Housenumber);
        }
      }
      if (!string.IsNullOrWhiteSpace(this.CityCode) || !string.IsNullOrWhiteSpace(this.City))
      {
        if (!string.IsNullOrWhiteSpace(this.Street) || !string.IsNullOrWhiteSpace(this.Housenumber))
        {
          address = string.Concat(address, Environment.NewLine);
        }

        if (!string.IsNullOrWhiteSpace(this.CityCode))
        {
          address = string.Concat(address, this.CityCode);
        }

        if (!string.IsNullOrWhiteSpace(this.City))
        {
          if (!string.IsNullOrWhiteSpace(this.CityCode))
          {
            address = string.Concat(address, " ", this.City);
          }
          else
          {
            address = string.Concat(address, this.City);
          }
        }
      }
      return address;
    }

    public static implicit operator string(Address address)
    {
      if (address == null)
      {
        return string.Empty;
      }
      return address.ToString();
    }

    public static Result<Address> Create(string city, string cityCode, string street, string houseNumber)
    {
      if (string.IsNullOrWhiteSpace(city) &&
        string.IsNullOrWhiteSpace(cityCode) &&
        string.IsNullOrWhiteSpace(street) &&
        string.IsNullOrWhiteSpace(houseNumber))
      {
        return Result.Failure<Address>("Please provide at least one value of the address.");
      }
      return Result.Success(new Address(city, cityCode, street, houseNumber));
    }
  }
}