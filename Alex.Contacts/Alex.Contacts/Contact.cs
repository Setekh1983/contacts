using Alex.DddBasics;

using System;

namespace Alex.Contacts
{
  public sealed class Contact : AggregateRoot
  {
#pragma warning disable IDE0052 // Remove unread private members
    
    Address? _Address;
    Name? _Name;

#pragma warning restore IDE0052 // Remove unread private members

#pragma warning disable IDE0051 // Remove unused private members
    
    private Contact(Guid id)
      : base(id)
    {
    }

#pragma warning restore IDE0051 // Remove unused private members

    public Contact(Name name)
      : base()
    {
      _ = name ?? throw new ArgumentNullException(nameof(name));
      
      this._Name = name;
      
      this.ApplyEvent(new ContactCreated(this.Id, name.FirstName, name.LastName));
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

    public void CorrectName(Name name)
    {
      _ = name ?? throw new ArgumentNullException(nameof(name));

      this.ApplyEvent(new ContactNameCorrected(this.Id, name.FirstName, name.LastName));
    }

#pragma warning disable IDE0051 // Remove unused private members
    
    private void Apply(ContactAddressAdded domainEvent) =>
      this._Address = Address.Create(
        domainEvent.City, domainEvent.CityCode, domainEvent.Street, domainEvent.HouseNumber).Value;

    private void Apply(ContactAddressCorrected domainEvent) =>
      this._Address = Address.Create(
        domainEvent.City, domainEvent.CityCode, domainEvent.Street, domainEvent.HouseNumber).Value;

    private void Apply(ContactNameCorrected domainEvent) =>
      this._Name = Name.Create(domainEvent.Forename, domainEvent.LastName).Value;

    private void Apply(ContactCreated domainEvent) =>
      this._Name = Name.Create(domainEvent.Forename, domainEvent.LastName).Value;

#pragma warning restore IDE0051 // Remove unused private members
  }
}