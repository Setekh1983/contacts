using Alex.DddBasics.Test.Domain;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;

namespace Alex.DddBasics.Test.AggregateRootTests
{
  [TestClass]
  public class An_New_Aggregate
  {
    [TestMethod]
    public void Has_No_Changes()
    {
      var sut = new Citizen();
      IEnumerable<IDomainEvent> events = sut.GetChanges();

      events.Should().BeEmpty();
    }

    [TestMethod]
    public void Version_Is_Zero()
    {
      IPersistableAggregate sut = new Citizen();

      sut.OriginatingVersion.Should().Be(0);
    }
  }
}
