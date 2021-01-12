using Alex.Contacts.Service.Commands;
using Alex.Contacts.Service.Controllers;
using Alex.DddBasics;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Alex.Contacts.Service.Test
{
  [TestClass]
  public class Correcting_A_Contacts_Name : ControllerTestBase
  {
    [TestMethod]
    public void Requires_An_Name()
    {
      IRepository<Contact> repository = EventProvider.GetRepository<Contact>();
      Guid contactId = CreateContact("Fred", "Simpson");
      var command = new CorrectNameCommand(contactId, "Homer", "Simpson");
      var sut = new ContactController(repository);

      ActionResult result = sut.CorrectName(command).GetAwaiter().GetResult();

      result.Should().BeOfType<OkResult>();
      List<IDomainEvent> domainEvents = EventProvider.GetEvents<Contact>(contactId).GetAwaiter().GetResult();

      domainEvents.Should().HaveCount(2);
      domainEvents.Last().Should().Match<ContactNameCorrectedV1>(domainEvent =>
        domainEvent.FirstName == command.FirstName &&
        domainEvent.LastName == command.LastName);
    }
    [TestMethod]
    public void With_Null_As_A_Command_Causes_A_Bad_Request_Result()
    {
      IRepository<Contact> repository = EventProvider.GetRepository<Contact>();

      var sut = new ContactController(repository);

      ActionResult result = sut.CorrectName(null).GetAwaiter().GetResult();

      result.Should().NotBeNull();
      result.Should().BeOfType<BadRequestResult>();
    }

    [TestMethod]
    public void With_Missing_Contact_Id_Causes_Not_Found_Result()
    {
      IRepository<Contact> repository = EventProvider.GetRepository<Contact>();
      var command = new CorrectNameCommand(Guid.Empty, "Homer", "Simpson");
      var sut = new ContactController(repository);

      ActionResult result = sut.CorrectName(command).GetAwaiter().GetResult();

      result.Should().NotBeNull();
      result.Should().BeOfType<NotFoundResult>();
    }

    [TestMethod]
    public void With_Non_Existing_Contact_Id_Causes_Not_Found_Result()
    {
      IRepository<Contact> repository = EventProvider.GetRepository<Contact>();
      var contactId = Guid.NewGuid();
      var command = new CorrectNameCommand(contactId, "Homer", "Simpson");
      var sut = new ContactController(repository);

      ActionResult result = sut.CorrectName(command).GetAwaiter().GetResult();

      result.Should().NotBeNull();
      result.Should().BeOfType<NotFoundResult>();
    }

    [TestMethod]
    public void With_Missing_FirstName_Causes_Unprocessable_Entity_Result()
    {
      IRepository<Contact> repository = EventProvider.GetRepository<Contact>();
      Guid contactId = CreateContact("Fred", "Simpson");
      var command = new CorrectNameCommand(contactId, string.Empty, "Simpson");
      var sut = new ContactController(repository);

      ActionResult result = sut.CorrectName(command).GetAwaiter().GetResult();

      result.Should().NotBeNull();
      result.ShouldBeUnprocessableEntityResult("name", "Please provide a first name.");
    }

    [TestMethod]
    public void With_Missing_LastName_Causes_Unprocessable_Entity_Result()
    {
      IRepository<Contact> repository = EventProvider.GetRepository<Contact>();
      Guid contactId = CreateContact("Fred", "Simpson");
      var command = new CorrectNameCommand(contactId, "Homer", string.Empty);
      var sut = new ContactController(repository);

      ActionResult result = sut.CorrectName(command).GetAwaiter().GetResult();

      result.Should().NotBeNull();
      result.ShouldBeUnprocessableEntityResult("name", "Please provide a last name.");
    }

    [TestMethod]
    public void With_Null_As_FirstName_Causes_Unprocessable_Entity_Result()
    {
      IRepository<Contact> repository = EventProvider.GetRepository<Contact>();
      Guid contactId = CreateContact("Fred", "Simpson");
      var command = new CorrectNameCommand(contactId, null, "Simpson");
      var sut = new ContactController(repository);

      ActionResult result = sut.CorrectName(command).GetAwaiter().GetResult();

      result.Should().NotBeNull();
      result.ShouldBeUnprocessableEntityResult("name", "Please provide a first name.");
    }

    [TestMethod]
    public void With_Null_As_LastName_Causes_Unprocessable_Entity_Result()
    {
      IRepository<Contact> repository = EventProvider.GetRepository<Contact>();
      Guid contactId = CreateContact("Fred", "Simpson");
      var command = new CorrectNameCommand(contactId, "Homer", null);
      var sut = new ContactController(repository);

      ActionResult result = sut.CorrectName(command).GetAwaiter().GetResult();

      result.Should().NotBeNull();
      result.ShouldBeUnprocessableEntityResult("name", "Please provide a last name.");
    }
  }
}
