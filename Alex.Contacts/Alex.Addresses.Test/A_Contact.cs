using FluentAssertions;

using System;

using Xunit;

namespace Alex.Addresses.Test
{
  public class A_Contact
  {
    [Fact]
    public void Can_Contain_An_Address()
    {
      Address address = Address.Create("Springfield", "12345", "Evergreen Terrace", "742").Value;
      Name name = Name.Create("Homer", "Simpson").Value;
      Contact contact = new Contact(name);

      contact.SetAddress(address);

      contact.Address.Should().Be(address);
    }

    [Fact]
    public void Does_Not_Accept_A_Null_Address()
    {
      Name name = Name.Create("Homer", "Simpson").Value;
      Contact contact = new Contact(name);

      Action action = () => contact.SetAddress(null);

      action.Should().Throw<ArgumentNullException>();
    }
  }
}
