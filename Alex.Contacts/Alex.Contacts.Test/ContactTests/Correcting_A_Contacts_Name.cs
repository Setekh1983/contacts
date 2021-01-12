using Alex.DddBasics;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Linq;

namespace Alex.Contacts.ContactTest
{
  [TestClass]
  public class Correcting_A_Contacts_Name
  {
    [TestMethod]
    public void Requires_A_Name()
    {
      var name = Name.Create("Homer", "Simpson").Value;
      var wrongName = Name.Create("Fred", "Simpson").Value;

      var sut = new Contact(wrongName);
      sut.CorrectName(name);

      IEnumerable<IDomainEvent> events = sut.GetChanges();

      events.Should().HaveCount(2);
      events.First().Should().Match<ContactCreatedV1>(domainEvent =>
        domainEvent.FirstName == wrongName.FirstName &&
        domainEvent.LastName == wrongName.LastName);
      events.Last().Should().Match<ContactNameCorrectedV1>(domainEvent =>
        domainEvent.ContactId == sut.Id &&
        domainEvent.FirstName == name.FirstName &&
        domainEvent.LastName == name.LastName);
    }
  }
}
