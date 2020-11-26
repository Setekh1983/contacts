using Alex.DddBasics.Test.Domain;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace Alex.DddBasics.Test.EntityTests
{
  [TestClass]
  public class An_Entitys_HashCode
  {
    [TestMethod]
    public void Is_Always_The_Same_For_The_Same_Guid_And_Type()
    {
      var guid = Guid.NewGuid();
      var entity1 = new Citizen(guid);
      var entity2 = new Citizen(guid);

      entity1.GetHashCode().Should().Be(entity2.GetHashCode());
    }
    [TestMethod]
    public void Is_Different_For_The_Different_Guids_And_The_Same_Type()
    {
      var entity1 = new Citizen();
      var entity2 = new Citizen();

      entity1.GetHashCode().Should().NotBe(entity2.GetHashCode());
    }
    [TestMethod]
    public void Is_Different_For_The_Same_Guid_And_Different_Types()
    {
      var guid = Guid.NewGuid();
      var entity1 = new Citizen(guid);
      var entity2 = new Citizen2(guid);

      entity1.GetHashCode().Should().NotBe(entity2.GetHashCode());
    }
  }
}
