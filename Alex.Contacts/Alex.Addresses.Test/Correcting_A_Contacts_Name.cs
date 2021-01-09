using Alex.DddBasics;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Linq;

namespace Alex.Addresses.Test
{
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

  [TestClass]
  public class Changing_A_Contacts_Name
  {
    [TestMethod]
    public void Requires_A_Name()
    {
      var firstName = "Homer";
      var lastName = "Simpson";
      var wrongName = Name.Create("Fred", lastName).Value;
      var correctName = Name.Create(firstName, wrongName.LastName).Value;

      var sut = new Contact(wrongName);
      sut.CorrectName(correctName);

      IEnumerable<IDomainEvent> events = sut.GetChanges();

      events.Should().HaveCount(2);
      events.First().Should().BeOfType<ContactCreated>();
      events.Last().Should().Match<ContactNameCorrected>(domainEvent =>
        domainEvent.ContactId == sut.Id &&
        domainEvent.Forename == firstName &&
        domainEvent.LastName == lastName);
    }
  }

#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
}
