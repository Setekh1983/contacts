using Alex.Addresses;
using Alex.DddBasics;

using CSharpFunctionalExtensions;

using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace Alex.Contacts.Service.Controllers
{
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
  }
}
