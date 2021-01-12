using Alex.DddBasics;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Alex.Contacts.ContactTest
{
  [TestClass]
  public class Correcting_A_Contacts_Address
  {
    [TestMethod]
    public void Requires_An_Address()
    {
      var oldAddress = Address.Create("Shelbyville", "56789", "Shelby Street", "8745").Value;
      var newAddress = Address.Create("Springfield", "12345", "Evergreen Terrace", "742").Value;
      var name = Name.Create("Homer", "Simpson").Value;
      var sut = new Contact(name);

      sut.AddAddress(oldAddress);
      sut.CorrectAddress(newAddress);

      IEnumerable<IDomainEvent> events = sut.GetChanges();

      events.Should().HaveCount(3);
      events.First().Should().BeOfType<ContactCreatedV1>();
      events.Last().Should().Match<ContactAddressCorrectedV1>(domainEvents =>
        domainEvents.ContactId == sut.Id &&
        domainEvents.City == newAddress.City &&
        domainEvents.CityCode == newAddress.CityCode &&
        domainEvents.Street == newAddress.Street &&
        domainEvents.HouseNumber == newAddress.HouseNumber);
    }
    [TestMethod]
    public void When_No_Address_Exists_Raises_An_Error()
    {
      var address = Address.Create("Springfield", "12345", "Evergreen Terrace", "742").Value;
      var name = Name.Create("Homer", "Simpson").Value;
      var sut = new Contact(name);

      Action action = () => sut.CorrectAddress(address);

      action.Should().Throw<InvalidOperationException>()
        .WithMessage("There is no address to correct.");
    }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

    [TestMethod]
    public void Does_Not_Accept_A_Null_Address()
    {
      var name = Name.Create("Homer", "Simpson").Value;
      var sut = new Contact(name);

      Action action = () => sut.CorrectAddress(null);

      action.Should().Throw<ArgumentNullException>();
    }

#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
  }
}
