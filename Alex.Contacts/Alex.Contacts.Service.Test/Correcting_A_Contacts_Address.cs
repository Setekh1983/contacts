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
  public class Correcting_A_Contacts_Address : ControllerTestBase
  {
    [TestMethod]
    public void Requires_An_Address()
    {
      IRepository<Contact> repository = EventProvider.GetRepository<Contact>();
      Guid contactId = CreateContactWithAddress();
      var command = new CorrectAddressCommand(contactId, "Springfield", "12345", "Evergreen Terrace", "1234");
      var sut = new ContactController(repository);

      ActionResult result = sut.CorrectAddress(command).GetAwaiter().GetResult();

      result.Should().BeOfType<OkObjectResult>();
      List<IDomainEvent> domainEvents = EventProvider.GetEvents<Contact>(contactId).GetAwaiter().GetResult();

      domainEvents.Should().HaveCount(3);
      domainEvents.Last().Should().Match<ContactAddressCorrectedV1>(domainEvent =>
        domainEvent.City == command.City &&
        domainEvent.CityCode == command.CityCode &&
        domainEvent.Street == command.Street &&
        domainEvent.HouseNumber == command.HouseNumber);
    }
    [TestMethod]
    public void Without_Existing_Name_Causes_Unprocessable_Entity()
    {
      Guid contactId = CreateContact("Homer", "Simpson");
      var sut = new ContactController(EventProvider.GetRepository<Contact>());
      var command = new CorrectAddressCommand(contactId, "Springfield", "12345", "Evergreen Terrace", "1234");

      ActionResult result = sut.CorrectAddress(command).GetAwaiter().GetResult();

      result.Should().NotBeNull();
      result.ShouldBeUnprocessableEntityResult("address", "There is no address to correct.");
    }

    [TestMethod]
    public void With_Missing_Contact_Id_Causes_Not_Found_Result()
    {
      IRepository<Contact> repository = EventProvider.GetRepository<Contact>();

      var command = new CorrectAddressCommand(Guid.Empty, "Springfield", "12345", "Evergreen Terrace", "1234");

      var sut = new ContactController(repository);
      ActionResult result = sut.CorrectAddress(command).GetAwaiter().GetResult();

      result.Should().NotBeNull();
      result.Should().BeOfType<NotFoundObjectResult>();
    }

    [TestMethod]
    public void With_Non_Existing_Contact_Id_Causes_Not_Found_Result()
    {
      IRepository<Contact> repository = EventProvider.GetRepository<Contact>();
      var contactId = Guid.NewGuid();
      var command = new CorrectAddressCommand(contactId, "Springfield", "12345", "Evergreen Terrace", "1234");
      var sut = new ContactController(repository);

      ActionResult result = sut.CorrectAddress(command).GetAwaiter().GetResult();

      result.Should().NotBeNull();
      result.Should().BeOfType<NotFoundObjectResult>();
    }

    [TestMethod]
    public void With_All_Empty_Fields_Causes_Unprocessable_Entity_Result()
    {
      IRepository<Contact> repository = EventProvider.GetRepository<Contact>();
      Guid contactId = CreateContact("Homer", "Simpson");

      var command = new CorrectAddressCommand(contactId, string.Empty, string.Empty, string.Empty, string.Empty);

      var sut = new ContactController(repository);
      ActionResult result = sut.CorrectAddress(command).GetAwaiter().GetResult();

      result.Should().NotBeNull();
      result.ShouldBeUnprocessableEntityResult("address", "Please provide at least one value of the address.");
    }

    [TestMethod]
    public void With_All_Null_Fields_Causes_Unprocessable_Entity_Result()
    {
      IRepository<Contact> repository = EventProvider.GetRepository<Contact>();
      Guid contactId = CreateContact("Homer", "Simpson");

      var command = new CorrectAddressCommand(contactId, null, null, null, null);

      var sut = new ContactController(repository);
      ActionResult result = sut.CorrectAddress(command).GetAwaiter().GetResult();

      result.Should().NotBeNull();
      result.ShouldBeUnprocessableEntityResult("address", "Please provide at least one value of the address.");
    }
    
    static Guid CreateContactWithAddress()
    {
      Guid contactId = CreateContact("Homer", "Simpson");
      var controller = new ContactController(EventProvider.GetRepository<Contact>());

      _ = controller.AddAddress(new AddAddressCommand(contactId, "Shelbyville", "56789", "Shelby Street", "3456"))
        .GetAwaiter().GetResult();

      return contactId;
    }
  }
}
