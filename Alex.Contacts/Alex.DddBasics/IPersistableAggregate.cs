using System;
using System.Collections.Generic;

namespace Alex.DddBasics
{
  public interface IPersistableAggregate
  {
    Guid Id { get; }
    long OriginatingVersion { get; }

    IEnumerable<IDomainEvent> GetChanges();
    void ChangesSaved(long newVersion);
    void LoadFromEvents(IEnumerable<IDomainEvent> events, long version);
  }
}
