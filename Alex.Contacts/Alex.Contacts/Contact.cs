using Alex.DddBasics;

using CSharpFunctionalExtensions;

using System;

namespace Alex.Contacts
{
  public sealed class Contact : AggregateRoot
  {
#pragma warning disable IDE0052 // Remove unread private members

    Address? _Address;
    Name? _Name;

#pragma warning restore IDE0052 // Remove unread private members


    private Contact(Guid id)
      : base(id)
    {
    }

    public Contact(Name name)
      : base()
    {
      _ = name ?? throw new ArgumentNullException(nameof(name));

      this._Name = name;

      this.ApplyEvent(new ContactCreatedV1(this.Id, name.FirstName, name.LastName));
    }
    public Result CanCorrectAddress(Address address)
    {
      _ = address ?? throw new ArgumentNullException(nameof(address));

      return Result.SuccessIf(this._Address is not null, Properties.Resources.NoNameToCorrect);
    }
    public void CorrectAddress(Address address)
    {
      _ = address ?? throw new ArgumentNullException(nameof(address));
      Result result = this.CanCorrectAddress(address);

      if (result.IsFailure)
      {
        throw new InvalidOperationException(result.Error);
      }
      this.ApplyEvent(new ContactAddressCorrectedV1(
        this.Id, address.City, address.CityCode, address.Street, address.HouseNumber));
    }
    public void AddAddress(Address address)
    {
      _ = address ?? throw new ArgumentNullException(nameof(address));

      this.ApplyEvent(new ContactAddressAddedV1(
        this.Id, address.City, address.CityCode, address.Street, address.HouseNumber));
    }
    public void CorrectName(Name name)
    {
      _ = name ?? throw new ArgumentNullException(nameof(name));

      this.ApplyEvent(new ContactNameCorrectedV1(this.Id, name.FirstName, name.LastName));
    }

    private void Apply(ContactAddressAddedV1 domainEvent) =>
      this._Address = Address.Create(
        domainEvent.City, domainEvent.CityCode, domainEvent.Street, domainEvent.HouseNumber).Value;

    private void Apply(ContactAddressCorrectedV1 domainEvent) =>
      this._Address = Address.Create(
        domainEvent.City, domainEvent.CityCode, domainEvent.Street, domainEvent.HouseNumber).Value;

    private void Apply(ContactNameCorrectedV1 domainEvent) =>
      this._Name = Name.Create(domainEvent.FirstName, domainEvent.LastName).Value;

    private void Apply(ContactCreatedV1 domainEvent) =>
      this._Name = Name.Create(domainEvent.FirstName, domainEvent.LastName).Value;
  }
}