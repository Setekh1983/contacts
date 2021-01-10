using Alex.DddBasics;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Linq;

namespace Alex.Addresses.Test
{
  [TestClass]
  public class Changing_A_Contacts_Name
  {
    [TestMethod]
    public void Requires_A_Name()
    {
      var firstName = Name.Create("Homer").Value;
      var lastName = Name.Create("Simpson").Value;
      var wrongName = Name.Create("Fred").Value;

      var sut = new Contact(wrongName, lastName);
      sut.CorrectName(firstName, lastName);

      IEnumerable<IDomainEvent> events = sut.GetChanges();

      events.Should().HaveCount(2);
      events.First().Should().BeOfType<ContactCreated>();
      events.Last().Should().Match<ContactNameCorrected>(domainEvent =>
        domainEvent.ContactId == sut.Id &&
        domainEvent.Forename == firstName &&
        domainEvent.LastName == lastName);
    }
  }
}
