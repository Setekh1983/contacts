using Alex.DddBasics.Test.Domain;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;

namespace Alex.DddBasics.Test.AggregateRootTests
{
  [TestClass]
  public class Saving_Changes
  {
    [TestMethod]
    public void Clears_The_Changes_And_Sets_The_New_Version()
    {
      var homer = new Citizen();
      var marge = new Citizen();
      var address = new Address("Springfield", "12345", "Evergreen Terrace", "123", "USA");

      homer.Marry(marge);
      homer.Move(address);

      IPersistableAggregate aggregate = homer;
      aggregate.ChangesSaved(2);

      aggregate.GetChanges().Should().BeEmpty();
      aggregate.OriginatingVersion.Should().Be(2);
    }

    [TestMethod]
    public void Setting_A_Lower_Version_Then_The_Current_Causes_Error()
    {
      var sut = new Citizen();
      var partner = new Citizen();

      List<IDomainEvent> events = new List<IDomainEvent>();
      events.Add(new CitizenMovedEvent(sut.Id, "Springfield", "12345", "Evergreeen Terrace", "1234", "USA"));
      events.Add(new CitizenMarriedEvent(sut.Id, partner.Id));

      ((IPersistableAggregate)sut).LoadFromEvents(events, 1);

      Action action = () => ((IPersistableAggregate)sut).ChangesSaved(0);

      action.Should().Throw<ArgumentException>("The new version must be greater than the previous version.");
    }
  }
}
