using Alex.DddBasics;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using System;
using System.Threading.Tasks;

namespace Alex.Contacts.Service.Utiliites
{
  public class ContactsControllerBase<TAggregateRoot> : ControllerBase
    where TAggregateRoot : AggregateRoot
  {
    public ContactsControllerBase(IRepository<TAggregateRoot> repository)
    {
      _ = repository ?? throw new ArgumentNullException(nameof(repository));

      this.Repository = repository;
    }

    protected IRepository<TAggregateRoot> Repository
    {
      get;
    }

    public async Task<ActionResult> Created(string routeName, TAggregateRoot result)
    {
      await this.Repository.SaveAsync(result);

      var envelope = ResponseEnvelope.Created(new { result.Id });

      return this.Created(routeName, envelope);
    }

    public async Task<OkObjectResult> Ok(TAggregateRoot result)
    {
      await this.Repository.SaveAsync(result);
      
      var envelope = ResponseEnvelope.Ok(result);

      return base.Ok(envelope);
    }

    new public NotFoundObjectResult NotFound()
    {
      var envelope = ResponseEnvelope.NotFound();

      return this.NotFound(envelope);
    }

    public override UnprocessableEntityObjectResult UnprocessableEntity(ModelStateDictionary modelState)
    {
      var envelope = ResponseEnvelope.UnprocessableEntity(modelState);

      return this.UnprocessableEntity(envelope);
    }
  }
}
