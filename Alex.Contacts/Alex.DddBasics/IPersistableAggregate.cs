using System;
using System.Collections.Generic;
using System.Text;

namespace Alex.DddBasics
{
  public interface IPersistableAggregate
  {
    Guid Id { get; }
    long OriginatingVersion { get; }

    IEnumerable<IDomainEvent> GetChanges();
    void ChangesSaved(long newVersion);
  }
}
