using FluentAssertions;

using System;

using Xunit;

namespace Alex.Addresses.Test
{
  public class Creating_A_Contact
  {
    [Fact]
    public void With_Minimal_Parameters()
    {
      Name name = Name.Create("Homer", "Simpson").Value;

      Contact contact = new Contact(name);

      contact.Should().NotBeNull();
    }

    [Fact]
    public void Requires_A_Name()
    {
      Action action = () => new Contact(null);

      action.Should().Throw<ArgumentNullException>();
    }
  }
}
