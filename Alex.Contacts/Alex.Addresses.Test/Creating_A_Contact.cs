using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace Alex.Addresses.Test
{
#pragma warning disable CA1806 // Do not ignore method results
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
  [TestClass]
  public class Creating_A_Contact
  {
    [TestMethod]
    public void With_Minimal_Parameters()
    {
      var name = Name.Create("Homer", "Simpson").Value;

      var sut = new Contact(name);

      sut.Should().NotBeNull();
    }

    [TestMethod]
    public void Requires_A_Name()
    {
      Action action = () => new Contact(null);

      action.Should().Throw<ArgumentNullException>();
    }
  }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning restore CA1806 // Do not ignore method results
}
