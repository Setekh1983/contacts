using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace Alex.Contacts.Service.ExceptionHandling
{
  public static partial class ExceptionHandlerMiddlewareExtensions
  {
    public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger<Startup> logger)
    {
      app.UseExceptionHandler(appError =>
      {
        appError.Run(async context =>
        {
          context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
          context.Response.ContentType = MediaTypeNames.Application.Json;

          var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

          if (exceptionHandlerFeature != null)
          {
            logger.LogError(exceptionHandlerFeature.Error, "An unhandled exception occurred");
            var errorDetails = new ErrorDetails(context.Response.StatusCode, "Internal Server Error");
            await context.Response.WriteAsync(JsonSerializer.Serialize(errorDetails));
          }
        });
      });
    }
  }
}
