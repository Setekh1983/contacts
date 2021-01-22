using Alex.Contacts.Service.Commands;
using Alex.Contacts.Service.Utiliites;
using Alex.DddBasics;

using CSharpFunctionalExtensions;

using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace Alex.Contacts.Service.Controllers
{
  [ApiController]
  [Route("[controller]")]
  [Route("[controller]/[action]")]
  public class ContactController : ContactsControllerBase<Contact>
  {
    public ContactController(IRepository<Contact> repository)
      : base(repository)
    {
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreateContactCommand command)
    {
      Result<Name> nameResult = Name.Create(command.FirstName, command.LastName);

      if (nameResult.IsFailure)
      {
        this.ModelState.AddModelError("name", nameResult.Error);
        return this.UnprocessableEntity(this.ModelState);
      }
      var contact = new Contact(nameResult.Value);
      return await this.Created("dummyRoute", contact);
    }

    [HttpPost]
    public async Task<ActionResult> CorrectName(CorrectNameCommand command)
    {
      Contact contact = await this.Repository.LoadAsync(command.ContactId);

      if (contact is null)
      {
        return this.NotFound();
      }
      Result<Name> nameResult = Name.Create(command.FirstName, command.LastName);

      if (nameResult.IsFailure)
      {
        this.ModelState.AddModelError("name", nameResult.Error);
        return this.UnprocessableEntity(this.ModelState);
      }
      contact.CorrectName(nameResult.Value);

      return await this.Ok(contact);
    }

    [HttpPost]
    public async Task<ActionResult> AddAddress(AddAddressCommand command)
    {
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

      return await this.Ok(contact);
    }

    [HttpPost]
    public async Task<ActionResult> CorrectAddress(CorrectAddressCommand command)
    {
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
      Result canCorrectAddress = contact.CanCorrectAddress(addressResult.Value);

      if (canCorrectAddress.IsFailure)
      {
        this.ModelState.AddModelError("address", canCorrectAddress.Error);
        return this.UnprocessableEntity(this.ModelState);
      }
      contact.CorrectAddress(addressResult.Value);

      return await this.Ok(contact);
    }
  }
}
