using System;

namespace Alex.Contacts.Service.Commands
{
  public sealed record CreateContactCommand(string Forename, string Surname);
  public sealed record AddAddressCommand(Guid ContactId, string City, string CityCode, string Street, string HouseNumber);
  public sealed record CorrectAddressCommand(Guid ContactId, string City, string CityCode, string Street, string HouseNumber);
}
