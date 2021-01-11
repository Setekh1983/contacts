using Alex.DddBasics;

using CSharpFunctionalExtensions;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Alex.Contacts.ContactTest
{
  [TestClass]
  public class Validating_Whether_An_Address_Can_Be_Corrected
  {
    [TestMethod]
    public void Confirms_When_An_Address_Exists_Already()
    {
      var oldAddress = Address.Create("Shelbyville", "56789", "Shelby Street", "8745").Value;
      var newAddress = Address.Create("Springfield", "12345", "Evergreen Terrace", "742").Value;
      var name = Name.Create("Homer", "Simpson").Value;
      var sut = new Contact(name);

      sut.AddAddress(oldAddress);
      Result result = sut.CanCorrectAddress(newAddress);

      result.IsSuccess.Should().BeTrue();
    }
    [TestMethod]
    public void Declines_When_No_Address_Exists_Already()
    {
      var newAddress = Address.Create("Springfield", "12345", "Evergreen Terrace", "742").Value;
      var name = Name.Create("Homer", "Simpson").Value;
      var sut = new Contact(name);

      Result result = sut.CanCorrectAddress(newAddress);

      result.IsFailure.Should().BeTrue();
      result.Error.Should().Be("There is no address to correct.");
    }
    
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

    [TestMethod]
    public void Does_Not_Accept_A_Null_Address()
    {
      var name = Name.Create("Homer", "Simpson").Value;
      var sut = new Contact(name);

      Action action = () => sut.CanCorrectAddress(null);

      action.Should().Throw<ArgumentNullException>();
    }

#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
  }
}
