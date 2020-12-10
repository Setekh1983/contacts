using System;

namespace Alex.DddBasics.EventStoreDB.Test.Domain
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

  public sealed class Citizen : AggregateRoot
  {
    public Citizen()
    {
    }
    public Citizen(Guid id)
      : base(id)
    {
    }

    public Guid Partner { get; private set; }

    public Address Address { get; private set; }

    public void Move(Address newAddress) => this.ApplyEvent(new CitizenMovedEvent(
        this.Id, newAddress.City, newAddress.CityCode, newAddress.Street, newAddress.HouseNumber, newAddress.Country));

    public void Marry(Citizen citizen) => this.ApplyEvent(new CitizenMarriedEvent(this.Id, citizen.Id));

    private void Apply(CitizenMovedEvent evt) =>
      this.Address = new Address(evt.City, evt.CityCode, evt.Street, evt.HouseNumber, evt.Country);

    private void Apply(CitizenMarriedEvent evt) => this.Partner = evt.MarriedToCitizen;
  }

  public sealed record Address(string City, string CityCode, string Street, string HouseNumber, string Country);

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
