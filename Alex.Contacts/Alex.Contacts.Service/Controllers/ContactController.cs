using Alex.Contacts.Service.Commands;
using Alex.DddBasics;

using CSharpFunctionalExtensions;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

namespace Alex.Contacts.Service.Controllers
{
  [ApiController]
  public class ContactController : ControllerBase
  {
    public ContactController(IRepository<Contact> repository) => this.Repository = repository;

    public IRepository<Contact> Repository { get; }

    [HttpPost]
    public async Task<ActionResult> CreateContact(CreateContactCommand command)
    {
      if (command is null)
      {
        return this.BadRequest();
      }
      Result<Name> nameResult = Name.Create(command.Forename, command.Surname);

      if (nameResult.IsFailure)
      {
        this.ModelState.AddModelError("name", nameResult.Error);
        return this.UnprocessableEntity(this.ModelState);
      }
      var contact = new Contact(nameResult.Value);
      await this.Repository.SaveAsync(contact);

      return this.Created("dummyRoute", new { contact.Id });
    }

    [HttpPost]
    public async Task<ActionResult> AddAddress(AddAddressCommand command)
    {
      if (command is null)
      {
        return this.BadRequest();
      }
      Contact contact = await this.Repository.LoadAsync(command.ContactId);

      if (contact is null)
      {
        return this.NotFound();
      }
      Result<Address> addressResult = Address.Create(command.City, command.CityCode, command.Street, command.HouseNumber);

      if (addressResult.IsFailure)
      {
        this.ModelState.AddModelError("address", addressResult.Error);
        return this.UnprocessableEntity(this.ModelState);
      }
      contact.AddAddress(addressResult.Value);
      await this.Repository.SaveAsync(contact);

      return this.Ok();
    }

    [HttpPost]
    public async Task<ActionResult> CorrectAddress(CorrectAddressCommand command)
    {
      if (command is null)
      {
        return this.BadRequest();
      }
      Contact contact = await this.Repository.LoadAsync(command.ContactId);

      if (contact is null)
      {
        return this.NotFound();
      }
      Result<Address> addressResult = Address.Create(command.City, command.CityCode, command.Street, command.HouseNumber);

      if (addressResult.IsFailure)
      {
        this.ModelState.AddModelError("address", addressResult.Error);
        return this.UnprocessableEntity(this.ModelState);
      }
      contact.CorrectAddress(addressResult.Value);
      await this.Repository.SaveAsync(contact);

      return this.Ok();
    }
  }
}
