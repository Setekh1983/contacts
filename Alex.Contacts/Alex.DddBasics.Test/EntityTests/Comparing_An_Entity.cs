using Alex.DddBasics.Test.Domain;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace Alex.DddBasics.Test.EntityTests
{
  [TestClass]
  public class Comparing_An_Entity
  {
    [TestMethod]
    public void With_Null_is_False()
    {
      var sut = new Citizen();

      sut.Equals(null).Should().BeFalse();
    }
    [TestMethod]
    public void With_The_Same_Instance_Is_True()
    {
      var sut = new Citizen();

      sut.Equals(sut).Should().BeTrue();
    }
    [TestMethod]
    public void With_An_Entity_With_The_Same_Id_Is_True()
    {
      var guid = Guid.NewGuid();
      var sut1 = new Citizen(guid);
      var sut2 = new Citizen(guid);

      sut1.Equals(sut2).Should().BeTrue();
    }
    [TestMethod]
    public void With_An_Entity_With_A_Different_Id_Is_False()
    {
      var entity1 = new Citizen();
      var entity2 = new Citizen();

      entity1.Equals(entity2).Should().BeFalse();
    }
    [TestMethod]
    public void With_A_Different_Type_Is_False()
    {
      var entity1 = new Citizen();
      var entity2 = new object();

      entity1.Equals(entity2).Should().BeFalse();
    }

    [TestMethod]
    public void Via_Equality_Operator_When_Both_Values_Are_Null_Is_False()
    {
      Citizen? entity1 = null;
      Citizen? entity2 = null;

      (entity1 == entity2).Should().BeFalse();
    }
    [TestMethod]
    public void Via_Equality_Operator_When_One_Value_Is_Null_Is_False()
    {
      Citizen? entity1 = null;
      Citizen entity2 = new();

      (entity1 == entity2).Should().BeFalse();
    }
    [TestMethod]
    public void Via_Equality_Operator_With_Same_Instance_Is_True()
    {
      var entity = new Citizen();
      var entity2 = entity;

      (entity == entity2).Should().BeTrue();
    }
    [TestMethod]
    public void Via_Equality_Operator_With_Same_Id_And_Same_Type_Is_True()
    {
      var guid = Guid.NewGuid();
      var entity1 = new Citizen(guid);
      var entity2 = new Citizen(guid);

      (entity1 == entity2).Should().BeTrue();
    }
    [TestMethod]
    public void Via_Equality_Operator_With_Different_Ids_Is_False()
    {
      var entity1 = new Citizen();
      var entity2 = new Citizen();

      (entity1 == entity2).Should().BeFalse();
    }
    [TestMethod]
    public void Via_Equality_Operator_With_Different_Type_Is_False()
    {
      var entity1 = new Citizen();
      var entity2 = new object();

      (entity1 == entity2).Should().BeFalse();
    }
    [TestMethod]
    public void Via_Inequality_Operator_When_Both_Values_Are_Null_is_True()
    {
      Citizen? entity1 = null;
      Citizen? entity2 = null;

      (entity1 != entity2).Should().BeTrue();
    }
    [TestMethod]
    public void Via_Inequality_Operator_When_One_Value_Is_Null_Is_True()
    {
      Citizen? entity1 = null;
      var entity2 = new Citizen();

      (entity1 != entity2).Should().BeTrue();
    }
    [TestMethod]
    public void Via_Inequality_Operator_With_Same_Instance_Is_False()
    {
      var entity = new Citizen();
      var entity2 = entity;

      (entity2 != entity).Should().BeFalse();
    }
    [TestMethod]
    public void Via_Inequality_Operator_With_Same_Id_Is_False()
    {
      var guid = Guid.NewGuid();
      var entity1 = new Citizen(guid);
      var entity2 = new Citizen(guid);

      (entity1 != entity2).Should().BeFalse();
    }
    [TestMethod]
    public void Via_Inequality_Operator_With_Different_Ids_Is_True()
    {
      var entity1 = new Citizen();
      var entity2 = new Citizen();

      (entity1 != entity2).Should().BeTrue();
    }
    [TestMethod]
    public void Via_Inequality_Operator_With_Different_Types_Is_True()
    {
      var entity1 = new Citizen();
      var entity2 = new object();

      (entity1 != entity2).Should().BeTrue();
    }
  }
}
