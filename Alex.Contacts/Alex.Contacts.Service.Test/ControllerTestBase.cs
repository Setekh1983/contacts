using Alex.Contacts.Service.Commands;
using Alex.Contacts.Service.Controllers;
using Alex.DddBasics;

using Microsoft.AspNetCore.Mvc;

using System;

namespace Alex.Contacts.Service.Test
{
  public class ControllerTestBase
  {
    protected internal static Guid CreateContact(string forename, string surname)
    {
      IRepository<Contact> repository = EventProvider.GetRepository<Contact>();
      var command = new CreateContactCommand(forename, surname);
      var sut = new ContactController(repository);

      var result = (CreatedResult)sut.Create(command).GetAwaiter().GetResult();

      return result.GetId();
    }
  }
}
