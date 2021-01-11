using Alex.Contacts.Service.Controllers;
using Alex.DddBasics;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable CS8605 // Unboxing a possibly null value.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
namespace Alex.Contacts.Service.Test
{
  [TestClass]
  public class Creating_A_Contact
  {
    [TestMethod]
    public void Requires_A_Name()
    {
      IRepository<Contact> repository = EventProvider.GetRepository<Contact>();

      var command = new CreateContactCommand()
      {
        Forename = "Homer",
        LastName = "Simpson"
      };

      var sut = new ContactController(repository);

      var result = (CreatedResult)sut.CreateContact(command).GetAwaiter().GetResult();
      var id = (Guid)result.Value.GetType().GetProperty("Id").GetValue(result.Value);

      List<IDomainEvent> domainEvents = EventProvider.GetEvents<Contact>(id).GetAwaiter().GetResult();

      domainEvents.Should().HaveCount(1);
      domainEvents.First().Should().Match<ContactCreated>(domainEvent =>
        domainEvent.Forename == command.Forename &&
        domainEvent.LastName == command.LastName);
    }

    [TestMethod]
    public void With_Null_As_A_Command_Causes_A_Bad_Request_Result()
    {
      IRepository<Contact> repository = EventProvider.GetRepository<Contact>();

      var sut = new ContactController(repository);

      ActionResult result = sut.CreateContact(null).GetAwaiter().GetResult();

      result.Should().NotBeNull();
      result.Should().BeOfType<BadRequestResult>();
    }

    [TestMethod]
    public void With_Missing_Forename_Causes_Unprocessable_Enttiy_Result()
    {
      IRepository<Contact> repository = EventProvider.GetRepository<Contact>();
      var sut = new ContactController(repository);

      var command = new CreateContactCommand()
      {
        LastName = "Simpson"
      };

      ActionResult result = sut.CreateContact(command).GetAwaiter().GetResult();

      result.Should().NotBeNull();
      result.ShouldBeUnprocessableEntityResult("name", "Please provide a forename.");
    }

    [TestMethod]
    public void With_Missing_LastName_Causes_Unprocessable_Entity_Result()
    {
      IRepository<Contact> repository = EventProvider.GetRepository<Contact>();
      var sut = new ContactController(repository);

      var command = new CreateContactCommand()
      {
        Forename = "Homer"
      };
      ActionResult result = sut.CreateContact(command).GetAwaiter().GetResult();

      result.Should().NotBeNull();
      result.ShouldBeUnprocessableEntityResult("name", "Please provide a surname.");
    }
  }
}

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8605 // Unboxing a possibly null value.