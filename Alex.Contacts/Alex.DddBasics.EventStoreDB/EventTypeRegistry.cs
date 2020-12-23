using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Alex.DddBasics.EventStoreDB
{
  public class EventTypeRegistry
  {
    static readonly Type DomainEventInterfaceType = typeof(IDomainEvent);

    Dictionary<string, Type> Map { get; }

    private EventTypeRegistry(Dictionary<string, Type> map) => this.Map = map;

    public Type GetType(string eventTypeName)
    {
      if (this.Map.TryGetValue(eventTypeName, out Type eventType))
      {
        return eventType;
      }
      throw new InvalidOperationException(string.Format(Properties.Resources.EventTypeNotFound, eventTypeName));
    }
    public static EventTypeRegistry LoadFromAssemblies(IEnumerable<Assembly> assemblies)
    {
      _ = assemblies ?? throw new ArgumentNullException(nameof(assemblies));

      var map = new Dictionary<string, Type>();

      foreach (Assembly assembly in assemblies)
      {
        AddFromAssembly(map, assembly);
      }
      return new EventTypeRegistry(map);
    }
    public static EventTypeRegistry LoadFromAssembly(Assembly assembly)
    {
      _ = assembly ?? throw new ArgumentNullException(nameof(assembly));

      var map = new Dictionary<string, Type>();
      AddFromAssembly(map, assembly);

      return new EventTypeRegistry(map);
    }

    static void AddFromAssembly(Dictionary<string, Type> map, Assembly assembly)
    {
      Type[] candidateTypes = assembly.GetExportedTypes();

      foreach (Type candidateType in candidateTypes)
      {
        if (!candidateType.IsAbstract &&
          !candidateType.IsInterface &&
          !candidateType.IsGenericType &&
          DomainEventInterfaceType.IsAssignableFrom(candidateType))
        {
          map.Add(candidateType.Name.ToLower(), candidateType);
        }
      }
    }
  }
}