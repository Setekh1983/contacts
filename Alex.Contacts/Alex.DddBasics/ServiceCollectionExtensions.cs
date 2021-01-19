﻿using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Alex.DddBasics
{
  public static class ServiceCollectionExtensions
  {
    public static void UseDddBasics(this IServiceCollection services, Assembly aggregateRootAssembly, params Assembly[] handlerAssemblies)
    {
      _ = aggregateRootAssembly ?? throw new ArgumentNullException(nameof(aggregateRootAssembly));
      _ = handlerAssemblies ?? throw new ArgumentNullException(nameof(handlerAssemblies));

      services.AddSingleton<IDomainEventDispatcher>(serviceProvider => new DomainEventDispatcher(serviceProvider.GetService));

      var domainEvents = aggregateRootAssembly.GetTypes()
        .Where(type => type.IsClass && !type.IsAbstract && !type.IsGenericType && type.IsPublic
            && type.GetInterfaces().Contains(typeof(IDomainEvent)))
        .Select(domainEventType => new DomainEventTypeDescriptor(
            domainEventType, typeof(IDomainEventHandler<>).MakeGenericType(domainEventType)))
        .ToList();

      RegisterEventHandlers(services, aggregateRootAssembly, domainEvents);

      foreach (var handlerAssembly in handlerAssemblies)
      {
        RegisterEventHandlers(services, handlerAssembly, domainEvents);
      }
    }
    public static void UseDddBasics(this IServiceCollection services, Assembly aggregateRootAssembly)
    {
      _ = aggregateRootAssembly ?? throw new ArgumentNullException(nameof(aggregateRootAssembly));

      services.UseDddBasics(aggregateRootAssembly, Array.Empty<Assembly>());
    }

    private static void RegisterEventHandlers(IServiceCollection services, Assembly handlerAssembly, 
      List<DomainEventTypeDescriptor> domainEvents)
    {
      foreach (var domainEvent in domainEvents)
      {
        List<Type> domainEventHandlers = GetEventHandlersFor(handlerAssembly, domainEvent);

        foreach (var domainEventHandler in domainEventHandlers)
        {
          if ( !services
            .Where(desciptor => desciptor.ServiceType == domainEvent.HandlerType && desciptor.ImplementationType == domainEventHandler)
            .Any())
          {
            services.AddTransient(domainEvent.HandlerType, domainEventHandler);
          }
        }
      }
    }

    private static List<Type> GetEventHandlersFor(Assembly handlerAssembly, DomainEventTypeDescriptor domainEvent)
    {
      return handlerAssembly.GetTypes()
        .Where(type => type.IsClass && type.GetInterfaces().Contains(domainEvent.HandlerType))
        .ToList();
    }
  }
}
