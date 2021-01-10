using Alex.DddBasics;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Alex.Addresses.Test
{
  [TestClass]
  public class A_New_Contact
  {
    [TestMethod]
    public void Requires_A_Name()
    {
      var forename = Name.Create("Homer").Value;
      var lastName = Name.Create("Simpson").Value;

      var sut = new Contact(forename, lastName);

      sut.Should().NotBeNull();

      IEnumerable<IDomainEvent> events = sut.GetChanges();

      events.Should().HaveCount(1);
      events.First().Should().Match<ContactCreated>(domainEvent =>
        domainEvent.ContactId == sut.Id &&
        domainEvent.Forename == forename &&
        domainEvent.LastName == lastName);
    }

    [TestMethod]
    public void With_Null_As_A_Forename_Raises_An_Error()
    {
      var lastName = Name.Create("Simpson").Value;

      Action action = () => new Contact(null, lastName);

      action.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void With_Null_As_A_LastName_Raises_An_Error()
    {
      var firstName = Name.Create("Homer").Value;

      Action action = () => new Contact(firstName, null);

      action.Should().Throw<ArgumentNullException>();
    }
  }
}
