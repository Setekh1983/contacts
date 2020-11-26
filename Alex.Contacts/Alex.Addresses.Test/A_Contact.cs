using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace Alex.Addresses.Test
{
  [TestClass]
  public class A_Contact
  {
    [TestMethod]
    public void Can_Contain_An_Address()
    {
      Address address = Address.Create("Springfield", "12345", "Evergreen Terrace", "742").Value;
      Name name = Name.Create("Homer", "Simpson").Value;

      Contact sut = new Contact(name);

      sut.SetAddress(address);

      sut.Address.Should().Be(address);
    }

    [TestMethod]
    public void Does_Not_Accept_A_Null_Address()
    {
      Name name = Name.Create("Homer", "Simpson").Value;
      Contact sut = new Contact(name);

      Action action = () => sut.SetAddress(null);

      action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void Can_Correct_Its_Name()
    {
      Name wrongName = Name.Create("Fred", "Simpson").Value;
      Name correctName = Name.Create("Homer", wrongName.LastName).Value;

      Contact sut = new Contact(wrongName);
      sut.CorrectName(correctName);

      sut.Name.Should().Be(correctName);
    }
  }
}
