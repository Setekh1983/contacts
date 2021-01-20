using Alex.DddBasics;

using System;

namespace Alex.Contacts
{
  public class ContactAddressCorrectedV1 : DomainEvent
  {
    public string HouseNumber { get; }
    public string Street { get; }
    public string CityCode { get; }
    public string City { get; }
    public Guid ContactId { get; }

    public ContactAddressCorrectedV1(Guid contactId, string city, string cityCode, string street, string houseNumber)
    {
      this.ContactId = contactId;
      this.City = city;
      this.CityCode = cityCode;
      this.Street = street;
      this.HouseNumber = houseNumber;
    }
  }
}