using CSharpFunctionalExtensions;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alex.Addresses.Test
{
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8604 // Possible null reference argument.
  [TestClass]
  public class Casting_A_Name
  {
    [TestMethod]
    public void Containing_Null_Can_Be_Cast_To_An_Empty_String()
    {
      Name? sut = null;

      ((string)sut).Should().BeEmpty();
    }
  }
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
}
