using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alex.DddBasics
{
  public class DomainEventDispatcher : IDomainEventDispatcher
  {
    public async Task Dispatch(IDomainEvent domainEvent) => throw new NotImplementedException();
    public async Task Dispatch(IEnumerable<IDomainEvent> domainEvents) => throw new NotImplementedException();
  }
}
