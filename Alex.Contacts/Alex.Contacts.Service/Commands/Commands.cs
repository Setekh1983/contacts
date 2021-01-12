using System;

namespace Alex.Contacts.Service.Commands
{
  public sealed record CreateContactCommand(string FirstName, string LastName);
  public sealed record CorrectNameCommand(Guid ContactId, string FirstName, string LastName);
  public sealed record AddAddressCommand(Guid ContactId, string City, string CityCode, string Street, string HouseNumber);
  public sealed record CorrectAddressCommand(Guid ContactId, string City, string CityCode, string Street, string HouseNumber);
}
