using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace Alex.Addresses.Test
{
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

  [TestClass]
  public class Correcting_A_Contacts_Address
  {
    [TestMethod]
    public void Requires_An_Address()
    {
      var address = Address.Create("Springfield", "12345", "Evergreen Terrace", "742").Value;
      var name = Name.Create("Homer", "Simpson").Value;

      var sut = new Contact(name);

      sut.SetAddress(address);

      sut.Address.Should().Be(address);
    }

    [TestMethod]
    public void Does_Not_Accept_A_Null_Address()
    {
      var name = Name.Create("Homer", "Simpson").Value;
      var sut = new Contact(name);

      Action action = () => sut.SetAddress(null);

      action.Should().Throw<ArgumentNullException>();
    }
  }

#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
}
