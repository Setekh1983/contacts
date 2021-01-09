using Alex.DddBasics;

using System;

namespace Alex.Addresses
{
  public class ContactAddressCorrected : IDomainEvent
  {
    public string HouseNumber { get; }
    public string Street { get; }
    public string CityCode { get; }
    public string City { get; }
    public Guid ContactId { get; }

    public ContactAddressCorrected(Guid contactId, string city, string cityCode, string street, string houseNumber)
    {
      this.ContactId = contactId;
      this.City = city;
      this.CityCode = cityCode;
      this.Street = street;
      this.HouseNumber = houseNumber;
    }
  }
}