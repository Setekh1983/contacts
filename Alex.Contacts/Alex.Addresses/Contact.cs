using Alex.DddBasics;

using System;

namespace Alex.Addresses
{
  public class Contact : AggregateRoot
  {
    Address _Address;
    Name _FirstName;
    Name _LastName;

    private Contact(Guid id)
      : base(id)
    {

    }
    public Contact(Name firstName, Name lastName)
      : base()
    {
      _ = firstName ?? throw new ArgumentNullException(nameof(firstName));
      _ = lastName ?? throw new ArgumentNullException(nameof(lastName));

      this._FirstName = firstName;
      this._LastName = lastName;

      this.ApplyEvent(new ContactCreated(this.Id, firstName, lastName));
    }

    public void CorrectAddress(Address address)
    {
      _ = address ?? throw new ArgumentNullException(nameof(address));

      this.ApplyEvent(new ContactAddressCorrected(
        this.Id, address.City, address.CityCode, address.Street, address.HouseNumber));
    }

    public void AddAddress(Address address)
    {
      _ = address ?? throw new ArgumentNullException(nameof(address));

      this.ApplyEvent(new ContactAddressAdded(
        this.Id, address.City, address.CityCode, address.Street, address.HouseNumber));
    }

    public void CorrectName(Name firstName, Name lastName)
    {
      _ = firstName ?? throw new ArgumentNullException(nameof(firstName));
      _ = lastName ?? throw new ArgumentNullException(nameof(lastName));

      this.ApplyEvent(new ContactNameCorrected(this.Id, firstName, lastName));
    }

    private void Apply(ContactAddressAdded domainEvent) =>
      this._Address = Address.Create(
        domainEvent.City, domainEvent.CityCode, domainEvent.Street, domainEvent.HouseNumber).Value;

    private void Apply(ContactAddressCorrected domainEvent) =>
      this._Address = Address.Create(
        domainEvent.City, domainEvent.CityCode, domainEvent.Street, domainEvent.HouseNumber).Value;

    private void Apply(ContactNameCorrected domainEvent)
    {
      this._FirstName = Name.Create(domainEvent.Forename).Value;
      this._LastName = Name.Create(domainEvent.LastName).Value;
    }

    private void Apply(ContactCreated domainEvent)
    {
      this._FirstName = Name.Create(domainEvent.Forename).Value;
      this._LastName = Name.Create(domainEvent.LastName).Value;
    }
  }
}