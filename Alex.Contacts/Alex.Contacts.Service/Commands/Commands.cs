using System;

namespace Alex.Contacts.Service.Commands
{
  public sealed record AddAddressCommand(Guid ContactId, string City, string CityCode, string Street, string HouseNumber);
  public sealed record CreateContactCommand(string Forename, string Surname);
}
