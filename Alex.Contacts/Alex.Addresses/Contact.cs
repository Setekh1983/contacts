using Alex.DddBasics;

using System;

namespace Alex.Addresses
{
  public class Contact : AggregateRoot
  {
    private Contact(Guid id)
      : base(id)
    {

    }
    public Contact(Name name)
      : base()
    {
      _ = name ?? throw new ArgumentNullException(nameof(name));

      this.Name = name;

      this.ApplyEvent(new ContactCreated(this.Id, name.FirstName, name.LastName));
    }

    private Address Address { get; set; }
    private Name Name { get; set; }

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

    private void Apply(ContactAddressAdded domainEvent) =>
      this.Address = Address.Create(
        domainEvent.City, domainEvent.CityCode, domainEvent.Street, domainEvent.HouseNumber).Value;

    private void Apply(ContactAddressCorrected domainEvent) =>
      this.Address = Address.Create(
        domainEvent.City, domainEvent.CityCode, domainEvent.Street, domainEvent.HouseNumber).Value;

    private void Apply(ContactNameCorrected domainEvent) =>
      this.Name = Name.Create(domainEvent.Forename, domainEvent.LastName).Value;

    private void Apply(ContactCreated domainEvent) =>
      this.Name = Name.Create(domainEvent.Forename, domainEvent.LastName).Value;
  }
}