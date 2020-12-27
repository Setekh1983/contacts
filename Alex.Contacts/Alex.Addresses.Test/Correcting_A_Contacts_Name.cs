using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace Alex.Addresses.Test
{
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

  [TestClass]
  public class Changing_A_Contacts_Name
  {
    [TestMethod]
    public void Requires_A_Name()
    {
      var wrongName = Name.Create("Fred", "Simpson").Value;
      var correctName = Name.Create("Homer", wrongName.LastName).Value;

      var sut = new Contact(wrongName);
      sut.CorrectName(correctName);

      sut.Name.Should().Be(correctName);
    }
  }

#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
}
