using EventStore.Client;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Linq;

namespace Alex.DddBasics.EventStoreDB
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddEventStoreDB<TAggregateRoot>(
      this IServiceCollection services, Action<EventStoreOptions> configure) where TAggregateRoot : AggregateRoot
    {
      _ = configure ?? throw new ArgumentNullException(nameof(configure));

      var options = new EventStoreOptions();
      configure(options);

      if (string.IsNullOrEmpty(options.ConnectionString))
      {
        throw new ArgumentException("You have to provide a connection string", nameof(options.ConnectionString));
      }

      if (!options.EventAssemblies.Any())
      {
        throw new ArgumentException("You have to provide at least one domain event.", nameof(options.EventAssemblies));
      }

      services.AddSingleton(serviceProvider =>
      {
        var settings = EventStoreClientSettings.Create(options.ConnectionString);
        return new EventStoreClient(settings);
      });

      services.AddSingleton(ServiceProvider =>
      {
        return EventTypeRegistry.LoadFromAssemblies(options.EventAssemblies);
      });

      services.AddScoped(typeof(IRepository<TAggregateRoot>), typeof(Repository<TAggregateRoot>));

      return services;
    }
  }
}
