using Alex.DddBasics.EventStoreDB.Test.Domain;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace Alex.DddBasics.EventStoreDB.Test
{
  [TestClass]
  public class A_New_Repository : RepositoryTest
  {
    [TestMethod]
    public void Requires_An_EventStoreClient()
    {
      Action action = () => new Repository<Citizen>(null, GetEventTypeMap());

      action.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'eventStoreClient')");
    }

    [TestMethod]
    public void Requires_An_EventTypeMap()
    {
      Action action = () => new Repository<Citizen>(GetEventStoreClient(), null);

      action.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'eventTypeMap')");
    }

    [TestMethod]
    public void Is_Created_Properly()
    {
      var sut = new Repository<Citizen>(GetEventStoreClient(), GetEventTypeMap());

      sut.Should().NotBeNull();
    }
  }
}
