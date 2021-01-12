using Alex.Contacts.Service.Commands;
using Alex.Contacts.Service.Controllers;
using Alex.DddBasics;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alex.Contacts.Service.Test
{
  public class ControllerTestBase
  {
#pragma warning disable CS8605 // Unboxing a possibly null value.
#pragma warning disable CS8602 // Dereference of a possibly null reference.

    protected internal static Guid CreateContact(string forename, string surname)
    {
      IRepository<Contact> repository = EventProvider.GetRepository<Contact>();
      var command = new CreateContactCommand(forename, surname);
      var sut = new ContactController(repository);

      var result = (CreatedResult)sut.Create(command).GetAwaiter().GetResult();
      var id = (Guid)result.Value.GetType().GetProperty("Id").GetValue(result.Value);

      return id;
    }

#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8605 // Unboxing a possibly null value.
  }
}
