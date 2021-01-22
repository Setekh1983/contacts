
using Alex.Contacts.Service.Controllers;
using Alex.Contacts.Service.Utiliites;
using Alex.DddBasics.DependencyInjection;
using Alex.DddBasics.EventStoreDB;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Alex.Contacts.Service
{
  public class Startup
  {
    public Startup(IConfiguration configuration) => this.Configuration = configuration;

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers()
        .ConfigureCustomBadRequestResponse();

      services.AddEventStoreDB<Contact>(options =>
      {
        options.ConnectionString = this.Configuration["ConnectionStrings:EventStore"];
        options.EventAssemblies.Add(typeof(ContactCreatedV1).Assembly);
      });

      services.AddDddBasics(typeof(Contact).Assembly, typeof(ContactController).Assembly);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, ILogger<Startup> logger)
    {
      app.ConfigureExceptionHandler(logger);

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
