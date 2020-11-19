using Alex.DddBasics;

using System;

namespace Alex.DddBasics.Test.Domain
{
  public class CitizenMovedEvent : IDomainEvent
  {
    public CitizenMovedEvent(Guid citizen, string city, string cityCode, string street, string houseNumber, string country)
    {
      this.Citizen = citizen;
      this.City = city;
      this.CityCode = cityCode;
      this.Street = street;
      this.HouseNumber = houseNumber;
      this.Country = country;
    }

    public Guid Citizen { get; }
    public string City { get; }
    public string CityCode { get; }
    public string Street { get; }
    public string HouseNumber { get; }
    public string Country { get; }
  }
}