using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace Alex.Addresses.Test
{
#pragma warning disable CA1806 // Do not ignore method results
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
  [TestClass]
  public class A_New_Contact
  {
    [TestMethod]
    public void Requires_A_Name()
    {
      var name = Name.Create("Homer", "Simpson").Value;

      var sut = new Contact(name);

      sut.Should().NotBeNull();
    }

    [TestMethod]
    public void With_Null_As_A_Name_Raises_An_Error()
    {
      Action action = () => new Contact(null);

      action.Should().Throw<ArgumentNullException>();
    }
  }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning restore CA1806 // Do not ignore method results
}
