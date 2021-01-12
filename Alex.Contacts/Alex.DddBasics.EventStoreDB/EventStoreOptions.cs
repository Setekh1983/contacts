using System.Collections.Generic;
using System.Reflection;

namespace Alex.DddBasics.EventStoreDB
{
  public class EventStoreOptions
  {
    public string ConnectionString { get; set; }
    public IList<Assembly> EventAssemblies { get; }

    internal EventStoreOptions() => this.EventAssemblies = new List<Assembly>();
  }
}
