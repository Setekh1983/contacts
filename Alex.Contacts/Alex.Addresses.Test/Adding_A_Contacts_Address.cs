using Alex.DddBasics;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Alex.Addresses.Test
{
  [TestClass]
  public class Adding_A_Contacts_Address
  {
    [TestMethod]
    public void Requires_An_Address()
    {
      var address = Address.Create("Springfield", "12345", "Evergreen Terrace", "742").Value;
      var firstName = Name.Create("Homer").Value;
      var lastName = Name.Create("Simpson").Value;

      var sut = new Contact(firstName, lastName);

      sut.AddAddress(address);

      IEnumerable<IDomainEvent> events = sut.GetChanges();

      events.Should().HaveCount(2);
      events.First().Should().BeOfType<ContactCreated>();
      events.Last().Should().Match<ContactAddressAdded>(domainEvents =>
        domainEvents.ContactId == sut.Id &&
        domainEvents.City == address.City &&
        domainEvents.CityCode == address.CityCode &&
        domainEvents.Street == address.Street &&
        domainEvents.HouseNumber == address.HouseNumber);
    }

    [TestMethod]
    public void Does_Not_Accept_A_Null_Address()
    {
      var firstName = Name.Create("Homer").Value;
      var lastName = Name.Create("Simpson").Value;

      var sut = new Contact(firstName, lastName);

      Action action = () => sut.AddAddress(null);

      action.Should().Throw<ArgumentNullException>();
    }
  }
}
