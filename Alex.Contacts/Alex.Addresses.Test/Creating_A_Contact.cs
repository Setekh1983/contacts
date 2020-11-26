using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace Alex.Addresses.Test
{
  [TestClass]
  public class Creating_A_Contact
  {
    [TestMethod]
    public void With_Minimal_Parameters()
    {
      Name name = Name.Create("Homer", "Simpson").Value;

      Contact sut = new Contact(name);

      sut.Should().NotBeNull();
    }

    [TestMethod]
    public void Requires_A_Name()
    {
      Action action = () => new Contact(null);

      action.Should().Throw<ArgumentNullException>();
    }

  }
}
