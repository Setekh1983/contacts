using Alex.DddBasics.Test.Domain;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace Alex.DddBasics.Test.DomainEvents
{
  [TestClass]
  public class A_New_Domain_Event
  {
    [TestMethod]
    public void Has_An_Event_Id()
    {
      var domainEvent = new CitizenDiedEvent(Guid.NewGuid());

      domainEvent.EventId.Should().NotBeEmpty();
    }
  }
}
