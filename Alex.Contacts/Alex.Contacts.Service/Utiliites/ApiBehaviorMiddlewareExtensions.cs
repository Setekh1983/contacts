using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using System.Linq;

namespace Alex.Contacts.Service.Utiliites
{
  public static class ApiBehaviorMiddlewareExtensions
  {
    public static void ConfigureCustomBadRequestResponse(this IMvcBuilder builder)
    {
      builder.ConfigureApiBehaviorOptions(options =>
      {
        options.InvalidModelStateResponseFactory = actionContext =>
        {
          var envelope = ResponseEnvelope.BadRequest(actionContext.ModelState);
          return new BadRequestObjectResult(envelope);
        };
      });
    }
  }
}
