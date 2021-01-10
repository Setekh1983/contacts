using Alex.Addresses;
using Alex.DddBasics;

using CSharpFunctionalExtensions;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alex.Contacts.Service.Controllers
{
  public class ContactController : ControllerBase
  {
    public ContactController(IRepository<Contact> repository) => this.Repository = repository;

    public IRepository<Contact> Repository { get; }

    public async Task<ActionResult> CreateContact(CreateContactCommand command)
    {
      if (command is null)
      {
        return this.BadRequest();
      }
      Result<Name> forenameResult = Name.Create(command.Forename);

      if (forenameResult.IsFailure)
      {
        this.ModelState.AddModelError(nameof(command.Forename), forenameResult.Error);
      }
      Result<Name> lastNameResult = Name.Create(command.LastName);

      if (lastNameResult.IsFailure)
      {
        this.ModelState.AddModelError(nameof(command.LastName), lastNameResult.Error);
      }
      Result result = Result.Combine(forenameResult, lastNameResult);

      if (result.IsFailure)
      {
        return this.UnprocessableEntity(this.ModelState);
      }
      var contact = new Contact(forenameResult.Value, lastNameResult.Value);
      await this.Repository.SaveAsync(contact);

      return this.Created("dummyRoute", new { Id = contact.Id });
    }
  }
}
