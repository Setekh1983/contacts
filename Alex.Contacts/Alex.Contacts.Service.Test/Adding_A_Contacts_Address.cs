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
  public class Adding_A_Contacts_Address : ControllerTestBase
  {
    [TestMethod]
    public void Requires_An_Address()
    {
      IRepository<Contact> repository = EventProvider.GetRepository<Contact>();
      Guid contactId = CreateContact("Homer", "Simpson");
      var command = new AddAddressCommand(contactId, "Springfield", "12345", "Evergreen Terrace", "1234");
      var sut = new ContactController(repository);

      ActionResult result = sut.AddAddress(command).GetAwaiter().GetResult();

      result.Should().BeOfType<OkResult>();
      List<IDomainEvent> domainEvents = EventProvider.GetEvents<Contact>(contactId).GetAwaiter().GetResult();

      domainEvents.Should().HaveCount(2);
      domainEvents.Last().Should().Match<ContactAddressAdded>(domainEvent =>
        domainEvent.City == command.City &&
        domainEvent.CityCode == command.CityCode &&
        domainEvent.Street == command.Street &&
        domainEvent.HouseNumber == command.HouseNumber);
    }

    [TestMethod]
    public void With_Null_As_A_Command_Causes_A_Bad_Request_Result()
    {
      IRepository<Contact> repository = EventProvider.GetRepository<Contact>();
      var sut = new ContactController(repository);
      
      ActionResult result = sut.AddAddress(null).GetAwaiter().GetResult();

      result.Should().NotBeNull();
      result.Should().BeOfType<BadRequestResult>();
    }

    [TestMethod]
    public void With_Missing_Contact_Id_Causes_Not_Found_Result()
    {
      IRepository<Contact> repository = EventProvider.GetRepository<Contact>();
      var command = new AddAddressCommand(Guid.Empty, "Springfield", "12345", "Evergreen Terrace", "1234");
      var sut = new ContactController(repository);

      ActionResult result = sut.AddAddress(command).GetAwaiter().GetResult();

      result.Should().NotBeNull();
      result.Should().BeOfType<NotFoundResult>();
    }

    [TestMethod]
    public void With_Non_Existing_Contact_Id_Causes_Not_Found_Result()
    {
      IRepository<Contact> repository = EventProvider.GetRepository<Contact>();
      var contactId = Guid.NewGuid();
      var command = new AddAddressCommand(contactId, "Springfield", "12345", "Evergreen Terrace", "1234");
      var sut = new ContactController(repository);

      ActionResult result = sut.AddAddress(command).GetAwaiter().GetResult();

      result.Should().NotBeNull();
      result.Should().BeOfType<NotFoundResult>();
    }

    [TestMethod]
    public void With_All_Empty_Fields_Causes_Unprocessable_Entity_Result()
    {
      IRepository<Contact> repository = EventProvider.GetRepository<Contact>();
      Guid contactId = CreateContact("Homer", "Simpson");
      var command = new AddAddressCommand(contactId, string.Empty, string.Empty, string.Empty, string.Empty);
      var sut = new ContactController(repository);

      ActionResult result = sut.AddAddress(command).GetAwaiter().GetResult();

      result.Should().NotBeNull();
      result.ShouldBeUnprocessableEntityResult("address", "Please provide at least one value of the address.");
    }

    [TestMethod]
    public void With_All_Null_Fields_Causes_Unprocessable_Entity_Result()
    {
      IRepository<Contact> repository = EventProvider.GetRepository<Contact>();
      Guid contactId = CreateContact("Homer", "Simpson");
      var command = new AddAddressCommand(contactId, null, null, null, null);
      var sut = new ContactController(repository);

      ActionResult result = sut.AddAddress(command).GetAwaiter().GetResult();

      result.Should().NotBeNull();
      result.ShouldBeUnprocessableEntityResult("address", "Please provide at least one value of the address.");
    }
  }
}
