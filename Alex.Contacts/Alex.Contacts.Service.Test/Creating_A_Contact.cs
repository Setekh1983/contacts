using Alex.Contacts.Service.Commands;
using Alex.Contacts.Service.Controllers;
using Alex.DddBasics;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Linq;

namespace Alex.Contacts.Service.Test
{
  [TestClass]
  public class Creating_A_Contact
  {
    [TestMethod]
    public void Requires_A_Name()
    {
      IRepository<Contact> repository = EventProvider.GetRepository<Contact>();
      var command = new CreateContactCommand("Homer", "Simpson");
      var sut = new ContactController(repository);

      var result = (CreatedResult)sut.Create(command).GetAwaiter().GetResult();
      var id = result.GetId();

      List<IDomainEvent> domainEvents = EventProvider.GetEvents<Contact>(id).GetAwaiter().GetResult();

      domainEvents.Should().HaveCount(1);
      domainEvents.First().Should().Match<ContactCreatedV1>(domainEvent =>
        domainEvent.FirstName == command.FirstName &&
        domainEvent.LastName == command.LastName);
    }

    [TestMethod]
    public void With_Empty_Forename_Causes_Unprocessable_Enttiy_Result()
    {
      IRepository<Contact> repository = EventProvider.GetRepository<Contact>();
      var command = new CreateContactCommand(string.Empty, "Simpson");
      var sut = new ContactController(repository);

      ActionResult result = sut.Create(command).GetAwaiter().GetResult();

      result.Should().NotBeNull();
      result.ShouldBeUnprocessableEntityResult("name", "Please provide a first name.");
    }

    [TestMethod]
    public void With_Empty_LastName_Causes_Unprocessable_Entity_Result()
    {
      IRepository<Contact> repository = EventProvider.GetRepository<Contact>();
      var sut = new ContactController(repository);
      var command = new CreateContactCommand("Homer", string.Empty);

      ActionResult result = sut.Create(command).GetAwaiter().GetResult();

      result.Should().NotBeNull();
      result.ShouldBeUnprocessableEntityResult("name", "Please provide a last name.");
    }

    [TestMethod]
    public void With_Null_Forename_Causes_Unprocessable_Enttiy_Result()
    {
      IRepository<Contact> repository = EventProvider.GetRepository<Contact>();
      var command = new CreateContactCommand(null, "Simpson");
      var sut = new ContactController(repository);

      ActionResult result = sut.Create(command).GetAwaiter().GetResult();

      result.Should().NotBeNull();
      result.ShouldBeUnprocessableEntityResult("name", "Please provide a first name.");
    }

    [TestMethod]
    public void With_Null_LastName_Causes_Unprocessable_Entity_Result()
    {
      IRepository<Contact> repository = EventProvider.GetRepository<Contact>();
      var sut = new ContactController(repository);
      var command = new CreateContactCommand("Homer", null);

      ActionResult result = sut.Create(command).GetAwaiter().GetResult();

      result.Should().NotBeNull();
      result.ShouldBeUnprocessableEntityResult("name", "Please provide a last name.");
    }
  }
}